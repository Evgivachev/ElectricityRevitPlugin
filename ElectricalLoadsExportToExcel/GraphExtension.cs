using Excel = Microsoft.Office.Interop.Excel;


namespace ElectricalLoadsExportToExcel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;

    public static class GraphExtension
    {
        public static void MakeGraph(this Graph graph, IEnumerable<FamilyInstance> shields)
        {
            var queue = new Queue<Node>();
            foreach (var shield in shields)
            {
                var node = graph.AddNode(new Node(shield), true);
                queue.Enqueue(node);
            }

            while (queue.Count != 0)
            {
                var currentNode = queue.Dequeue();
                if (!graph.Contain(currentNode))
                    graph.AddNode(currentNode);
                var assignedSystems = currentNode
                    .Shield?
                    .MEPModel?
                    .GetAssignedElectricalSystems()?
                    .Cast<ElectricalSystem>();
                if (assignedSystems is null)
                    continue;
                foreach (var system in assignedSystems)
                {
                    var cm = system.ConnectorManager;
                    var connectors = cm.Connectors
                        .Cast<Connector>()
                        .SelectMany(c => c.AllRefs.Cast<Connector>());
                    foreach (var connector in connectors)
                    {
                        if (!(connector.Owner is FamilyInstance owner))
                            throw new NullReferenceException();
                        if (owner.Id.IntegerValue == currentNode.Shield!.Id.IntegerValue)
                            continue;
                        var doc = owner.Document;
                        var ownerCategory = owner.Category;
                        if (ownerCategory.Name == "Электрооборудование")
                        {
                            var elementNode = new Node(owner);
                            elementNode.PowerNode = graph[elementNode.PowerFamilyInstance.Name];
                            queue.Enqueue(elementNode);
                        }
                        else
                        {
                            var mepConnectorInfo = (MEPFamilyConnectorInfo)connector.GetMEPConnectorInfo();
                            var systemTypeParam =
                                (IntegerParameterValue)mepConnectorInfo.GetConnectorParameterValue(
                                    new ElementId(BuiltInParameter.RBS_ELEC_CIRCUIT_TYPE));
                            var clParam = (ElementIdParameterValue)mepConnectorInfo.GetConnectorParameterValue(
                                new ElementId(BuiltInParameter.RBS_ELEC_LOAD_CLASSIFICATION));
                            var cl = doc.GetElement(clParam.Value).Name;
                            if (cl.StartsWith("\\") || cl.StartsWith("/"))
                                continue;
                            var countOfPoles =
                                (IntegerParameterValue)mepConnectorInfo.GetConnectorParameterValue(
                                    new ElementId(BuiltInParameter.RBS_ELEC_NUMBER_OF_POLES));
                            var sParam = (DoubleParameterValue)mepConnectorInfo.GetConnectorParameterValue(
                                new ElementId(BuiltInParameter.RBS_ELEC_APPARENT_LOAD));
                            var s = UnitUtils.ConvertFromInternalUnits(sParam.Value, UnitTypeId.KilovoltAmperes);
                            var cosPhiParam =
                                (DoubleParameterValue)mepConnectorInfo.GetConnectorParameterValue(
                                    new ElementId(BuiltInParameter.RBS_ELEC_POWER_FACTOR));
                            var load = new Load(cl, s * cosPhiParam.Value, cosPhiParam.Value);
                            currentNode.AddLoad(load);
                        }
                    }
                }
            }
        }

        public static string Print(this Graph graph)
        {
            var strB = new StringBuilder();
            var i = 0;
            foreach (var node in graph.BaseNodes)
            {
                PrintNode(strB, node, i);
            }

            return strB.ToString();
        }

        private static void PrintNode(StringBuilder stringBuilder, Node node, int i)
        {
            var prefix = new StringBuilder();
            prefix.Append('\t', i);
            stringBuilder.Append($"{prefix}{node.Name}\n");
            foreach (var load in node.Loads)
            {
                stringBuilder.Append(
                    $"{prefix}{load.Value.Classification} {load.Value.P} {load.Value.CosPhi} {load.Value.Ks} {load.Value.Count}\n");
            }

            foreach (var incidentNode in node.IncidentNodes)
            {
                PrintNode(stringBuilder, incidentNode, i + 1);
            }
        }

        public static void UpdateGraph(this Graph graph)
        {
            //Очередь щитов для обхода сверху
            var queue = new Queue<Node>();
            foreach (var shield in graph.BaseNodes)
            {
                queue.Enqueue(shield);
            }

            while (queue.Count != 0)
            {
                var currentNode = queue.Dequeue();
                var queueChildShields = new Queue<Node>();
                foreach (var incidentNode in currentNode.IncidentNodes)
                {
                    queue.Enqueue(incidentNode);
                    queueChildShields.Enqueue(incidentNode);
                }

                while (queueChildShields.Count != 0)
                {
                    var childShield = queueChildShields.Dequeue();
                    var loadOfChildShield = childShield.Loads;
                    currentNode.AddLoad(loadOfChildShield.Values);
                    foreach (var incidentNode in childShield.IncidentNodes)
                    {
                        queueChildShields.Enqueue(incidentNode);
                    }
                }
            }
        }

        public static void UpdateGraphFromExcelFile(this Graph graph, Dictionary<string, List<Load>> dictionary)
        {
            var queue = new Queue<Node>();
            foreach (var baseShield in graph.BaseNodes)
            {
                queue.Enqueue(baseShield);
            }

            while (queue.Count != 0)
            {
                var currentNode = queue.Dequeue();
                foreach (var incidentNode in currentNode.IncidentNodes)
                {
                    queue.Enqueue(incidentNode);
                }

                if (!dictionary.ContainsKey(currentNode.Name))
                {
                    continue;
                }

                var excelLoads = dictionary[currentNode.Name];
                foreach (var load in currentNode.Loads.Values)
                {
                    var excelLoad = excelLoads.FirstOrDefault(x =>
                        string.Compare(x.Classification, load.Classification, StringComparison.InvariantCulture) == 0);
                    if (excelLoad is null)
                    {
                        continue;
                    }

                    load.Ks = excelLoad.Ks;
                }
            }
        }

        public static void ExportToExcel(this Graph graph, string filePath)
        {
            var objExcel = new Excel.Application();
            try
            {
                var objWorkBook = objExcel.Workbooks.Open(
                    filePath,
                    0, false,
                    5, "", "", false, Excel.XlPlatform.xlWindows,
                    "", true, false, 0, true, false, false);
                var objWorkSheet = (Excel.Worksheet)objWorkBook.Sheets.Add();
                //TODO имя листа
                objWorkSheet.Name = DateTime.Now.ToString("yy-MM-dd-HH-mm");
                //var objWorkSheet = (Excel.Worksheet)objWorkBook.Sheets[1];
                var currentCell = new Cell(1, 1);
                var queue = new Queue<Node>();
                foreach (var baseShield in graph.BaseNodes)
                {
                    queue.Enqueue(baseShield);
                }

                while (queue.Count != 0)
                {
                    var currentNode = queue.Dequeue();
                    foreach (var incidentNode in currentNode.IncidentNodes)
                    {
                        queue.Enqueue(incidentNode);
                    }

                    if (currentNode.Loads.Count < 1) continue;
                    objWorkSheet.PutShieldName(currentCell, currentNode.Name);
                    objWorkSheet.PutHead(currentCell);
                    objWorkSheet.PutLoads(currentCell, currentNode.Loads.Values, currentNode.U < 250 ? 1 : 3);
                }

                objExcel.DisplayAlerts = false;
                objWorkBook.Save();
            }
            finally
            {
                objExcel.Quit();
            }
        }
    }
}

namespace ElectricalLoadsExportToExcel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;

    public class Node
    {
        private readonly List<Node> _nodes = new List<Node>();
        public readonly SortedDictionary<string, Load> Loads = new SortedDictionary<string, Load>();
        public readonly FamilyInstance PowerFamilyInstance;
        public readonly FamilyInstance Shield;
        public readonly double U;
        public Node PowerNode;

        public Node(FamilyInstance shield)
        {
            Name = shield.Name;
            Shield = shield;
            var prefix = shield
                .LookupParameter("Префикс цепи")?
                .AsString();
            var powerCable = shield.GetPowerElectricalSystem();
            var powerNode = powerCable?.BaseEquipment;
            PowerFamilyInstance = powerNode;
            var uString = shield.LookupParameter("Напряжение в щите").AsValueString().Split(' ')[0];
            //if (double.TryParse(uString, out var u)&&u<100 || uString=="0") return false;
            double.TryParse(uString, out var u);
            U = u;
        }

        public string Name { get; }
        public double CountOfElements { get; set; }

        public IEnumerable<Node> IncidentNodes
        {
            get
            {
                foreach (var node in _nodes)
                {
                    yield return node;
                }
            }
        }


        public override string ToString()
        {
            return Name;
        }

        public static void Connect(Node childNode, Node parentNode, Graph graph)
        {
            if (!graph.Nodes.Contains(childNode) || !graph.Nodes.Contains(parentNode)) throw new ArgumentException();
            parentNode._nodes.Add(childNode);
            childNode.PowerNode = parentNode;
        }

        public Load AddLoad(Load load)
        {
            if (Loads.ContainsKey(load.Classification))
            {
                Loads[load.Classification] += load;
            }
            else
            {
                var newLoad = new Load(load);
                Loads.Add(load.Classification, newLoad);
            }

            CountOfElements++;
            return load;
        }

        public void AddLoad(IEnumerable<Load> loads)
        {
            foreach (var load in loads)
            {
                AddLoad(load);
            }
        }
    }
}

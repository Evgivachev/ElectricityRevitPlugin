namespace ElectricalLoadsExportToExcel
{
    using System.Collections.Generic;

    public class Graph
    {
        private readonly Dictionary<string, Node> _nodesDict = new Dictionary<string, Node>();
        public List<Node> BaseNodes = new List<Node>();

        public int Length => _nodesDict.Count;

        public Node this[string index]
        {
            get
            {
                if (!_nodesDict.ContainsKey(index))
                    return null;
                return _nodesDict[index];
            }
        }

        public IEnumerable<Node> Nodes
        {
            get
            {
                foreach (var node in _nodesDict) yield return node.Value;
            }
        }

        public Node AddNode(Node node, bool baseNode = false)
        {
            if (baseNode)
            {
                BaseNodes.Add(node);
            }

            _nodesDict.Add(node.Name, node);
            if (Contain(node.PowerNode))
            {
                Connect(node.Name, node.PowerNode.Name);
            }

            return node;
        }

        public bool Contain(Node node)
        {
            return !(node is null) && _nodesDict.ContainsKey(node.Name);
        }

        public void Connect(string child, string parent)
        {
            Node.Connect(_nodesDict[child], _nodesDict[parent], this);
        }
    }
}

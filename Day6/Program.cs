using System;
using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    public class Program
    {
        public static SolarSystem solarSystem { get; set; }
        static void Main(string[] args)
        {
            var orbits = System.IO.File.ReadAllLines(@"E:\Downloads\dev\advent-of-code-2019\inputs\day6-1.txt");
            solarSystem = SolarSystem.Instance;

            var tree = InitializeTree(orbits);
            int totalNrOfOrbits = GetAllOrbits();
            Console.WriteLine($"Total number of orbit connections: {totalNrOfOrbits}");
            var you = solarSystem.Objects.Single(o => o.Name == "YOU");
            var santa = solarSystem.Objects.Single(o => o.Name == "SAN");
            if (you.Parent != santa.Parent)
            {
                var traveledNodesFromYOU = ShortestPath(new List<Node>(), you.Parent, santa.Parent);
                var traveledNodesFromSANTA = ShortestPath(new List<Node>(), santa.Parent, you.Parent, traveledNodesFromYOU);
                //var traveledNodes = ShortestPath(new List<Node>(), you.Parent, new List<Node>(), santa.Parent);
                //Console.WriteLine($"Shortest orbit transfers: {traveledNodes.Count - 1}");
                var crossingNode = traveledNodesFromSANTA.Last();
                var shortestTransfer = (traveledNodesFromSANTA.Count - 1) + (traveledNodesFromYOU.GetRange(0, traveledNodesFromYOU.IndexOf(crossingNode)).Count);
            }
        }

        static List<Node> ShortestPath(List<Node> traveledNodes, Node currentNode, Node targetNode, List<Node> compareList)
        {
            if (currentNode == null)
            {
                return traveledNodes;
            }
            traveledNodes.Add(currentNode);

            if (currentNode == targetNode)
            {
                return traveledNodes;
            }

            if (compareList.Contains(currentNode))
            {
                return traveledNodes;
            }

            return ShortestPath(traveledNodes, currentNode.Parent, targetNode, compareList);
        }

        static List<Node> ShortestPath(List<Node> traveledNodes, Node currentNode, Node targetNode)
        {
            if (currentNode == null)
            {
                return traveledNodes;
            }
            traveledNodes.Add(currentNode);

            if (currentNode == targetNode)
            {
                return traveledNodes;
            }

            traveledNodes = ShortestPath(traveledNodes, currentNode.Parent, targetNode);
            return traveledNodes;
        }

        static List<Node> ShortestPath(List<Node> traveledNodes1, Node node1, List<Node> traveledNodes2, Node node2)
        {
            // Check if we are at the end of the tree
            if (node1 == null)
            {
                return traveledNodes1;
            }

            if (traveledNodes2.Contains(node1))
            {
                var index = traveledNodes2.IndexOf(node1) + 1;
                traveledNodes1.AddRange(traveledNodes2.GetRange(0, index));
                return traveledNodes1;
            }
            traveledNodes1.Add(node1);


            if (node1 != node2)
            {
                traveledNodes1 = ShortestPath(traveledNodes1, node1.Parent, traveledNodes2, node2);
            }

            if (!traveledNodes1.Contains(node2))
            {
                traveledNodes2.Add(node2);
                traveledNodes2 = ShortestPath(traveledNodes2, node2.Parent, traveledNodes1, node1);
            }



            return traveledNodes2;
        }

        static int GetAllOrbits()
        {
            int totalNrOfOrbits = 0;

            foreach (var node in solarSystem.Objects)
            {
                totalNrOfOrbits += GetOrbits(0, node);
            }

            return totalNrOfOrbits;
        }

        static int GetOrbits(int prevOrbits, Node node)
        {
            if (node.Parent != null)
            {
                return GetOrbits(++prevOrbits, node.Parent);
            }
            return prevOrbits;
        }



        static List<Node> InitializeTree(string[] orbits)
        {
            var tree = new List<Node>();


            foreach (var orbit in orbits)
            {
                var objects = orbit.Split(')');
                if (objects.Length != 2)
                {
                    throw new Exception("Wrongly formatted input.");
                }

                var stationaryObject = objects[0];
                var orbitingObject = objects[1];
                AddOrbit(stationaryObject, orbitingObject);

            }
            return null;
        }

        private static void AddOrbit(string stationaryObjectName, string orbitingObjectName)
        {

            Node parent = solarSystem.GetObject(stationaryObjectName);
            if (parent == null)
            {
                parent = new Node(stationaryObjectName);
                solarSystem.AddObject(parent);
            }

            Node orbitingObject = solarSystem.GetObject(orbitingObjectName);
            if (orbitingObject == null)
            {
                orbitingObject = new Node(parent, orbitingObjectName);
                solarSystem.AddObject(orbitingObject);
            }
            else if (orbitingObject.Parent == null)
            {
                orbitingObject.AddParent(parent);
            }

            parent.AddChild(orbitingObject);
        }
    }

    public class SolarSystem
    {
        public List<Node> Objects { get; private set; }
        private static SolarSystem instance = null;

        private SolarSystem()
        {
            Objects = new List<Node>();
        }

        public static SolarSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    return new SolarSystem();
                }
                return instance;
            }
        }

        public void AddObject(Node node)
        {
            Objects.Add(node);
        }

        public Node GetObject(string name)
        {
            var node = Objects.FirstOrDefault(o => o.Name == name);
            return node;
        }

    }

    public class Node
    {
        public string Name { get; private set; }
        public Node Parent { get; private set; }
        public List<Node> Children { get; set; }

        public Node(string name)
        {
            Children = new List<Node>();
            Name = name;
        }

        public Node(Node parent, string name) : this(name)
        {
            Parent = parent;
        }

        public void AddParent(Node parent)
        {
            Parent = parent;
        }

        public void AddChild(Node child)
        {
            Children.Add(child);
        }
    }
}

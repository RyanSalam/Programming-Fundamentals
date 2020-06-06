using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph<T>
{
    public Graph()
    {
        nodes = new List<Node<T>>();
    }

    public class Node<NodeType>
    {

        public Node(NodeType data)
        {
            value = data;
        }

        public NodeType GetData()
        {
            return value;
        }

        public List<Node<NodeType>> GetIncoming()
        {
            return incoming;
        }

        public List<Node<NodeType>> GetOutgoing()
        {
            return outgoing;
        }

        public void AddIncoming(Node<NodeType> node)
        {
            incoming.Add(node);
        }

        public void AddOutgoing(Node<NodeType> node)
        {
            outgoing.Add(node);
        }

        private NodeType value;
        private List<Node<NodeType>> incoming  = new List<Node<NodeType>>();
        private List<Node<NodeType>> outgoing = new List<Node<NodeType>>();
    }

    private List<Node<T>> nodes;

    public Node<T> AddNode(T data)
    {
        Node<T> node = new Node<T>(data);

        nodes.Add(node);

        return node;
    }

    public Node<T> FindNode(T data)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].GetData().Equals(data))
                return nodes[i];
        }

        return null;
    }

    public void AddEdge(Node<T> source, Node<T> destination)
    {
        if (source == null || destination == null)
            return;

        source.AddOutgoing(destination);

        destination.AddIncoming(source);
    }

    public void AddEdge(T source, T dest)
    {
        AddEdge(FindNode(source), FindNode(dest));
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {

        public Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract Node SetupTree();

        public void setRoot(List<Node> children, Node root)
        {
            foreach (Node node in children)
            {
                node.root = root;
                if (node.children.Count > 0)
                {
                    setRoot(node.children, root);
                }
            }
        }

        public void setTreeRef(List<Node> children, Tree tree)
        {
            foreach (Node node in children)
            {
                node.referenceTree = tree;
                if (node.children.Count > 0)
                {
                    setTreeRef(node.children, tree);
                }
            }
        }

    }

}

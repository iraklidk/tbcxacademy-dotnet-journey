namespace Practice
{
    public class Node<T>
    {
        public T Value;
        public Node<T> Left;
        public Node<T> Right;
        public int Height; // for AVL tracking
        public Node(T value, Node<T> left = null, Node<T> right = null)
        {
            Value = value;
            Height = 1;
        }
    }

    public class GenericBST<T> where T : IComparable<T>
    {
        public Node<T> Root;
        public GenericBST() { Root = null; }

        public Node<T> Search(T value) // returns null if not found
        {
            return SearchDFS(Root, value);
        }
        public Node<T> SearchDFS(Node<T> root, T value)
        {
            if(root is null) return null;
            if(root.Value.CompareTo(value) == 0) return root;
            else if(value.CompareTo(root.Value) < 0) return SearchDFS(root.Left, value);
            else return SearchDFS(root.Right, value);
        }

        public void Insert(T value)
        {
            Root = InsertDFS(Root, value);
        }
        private Node<T> InsertDFS(Node<T> root, T value)
        {
            if(root is null) return new Node<T>(value);
            else if(value.CompareTo(root.Value) < 0) root.Left = InsertDFS(root.Left, value);
            else root.Right = InsertDFS(root.Right, value);

            return root.BalanceNode(); // ipdate height and rebalance avl subtree
        }

        public void Delete(T value) => Root = DeleteDFS(Root, value);
        private Node<T> DeleteDFS(Node<T> root, T value)
        {
            if(root is null) return root;

            root.Left = DeleteDFS(root.Left, value);
            root.Right = DeleteDFS(root.Right, value);

            if(root.Value.CompareTo(value) == 0)
            {
                if(root.Left is null) return root.Right;
                if(root.Right is null) return root.Left;
                Node<T> minNode = GetMinNode(root.Right); // get right subtree's minimum node
                root.Value = minNode.Value;
                root.Right = DeleteDFS(root.Right, minNode.Value);
            }

            return root.BalanceNode(); // rebalance
        }
        private Node<T> GetMinNode(Node<T> node)
        {
            if(node.Left is null) return node;
            return GetMinNode(node.Left);
        }

        public void PrintBSTAsSortedArray()
        {
            InOrder(Root);
        }
        private void InOrder(Node<T> root)
        {
            if (root is null) return;
            InOrder(root.Left);
            Console.Write(root.Value + " ");
            InOrder(root.Right);
        }

    }
}
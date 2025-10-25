namespace Practice
{
    public class GenericStack<T>
    {
        T[] array;
        private int size, AllocatedMemory;
        public int Length { get => size; }
        public GenericStack() { }

        public void Push(T item)
        {
            if (AllocatedMemory == size)
            {
                T[] newArray = new T[AllocatedMemory == 0 ? 4 : AllocatedMemory * 2]; // resize (exactly like in GenericQueue)
                for (int i = 0; i < size; ++i) newArray[i] = array[i];
                AllocatedMemory = newArray.Length;
                array = newArray;
            }
            array[size++] = item;
        }

        public T Pop()
        {
            if (size == 0) throw new InvalidOperationException("Stack is empty");
            return array[--size];
        }

        public T Peek()
        {
            if (size == 0) throw new InvalidOperationException("Stack is empty");
            return array[size - 1];
        }

        public void Clear()
        {
            size = 0;
            array = Array.Empty<T>();
            AllocatedMemory = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < size; ++i)
            {
                if (EqualityComparer<T>.Default.Equals(array[i], item))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
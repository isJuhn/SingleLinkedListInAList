using System;
using System.Collections;
using System.Collections.Generic;

namespace SingleLinkedListInAList
{
    class ListInAList<T> : IList<T>//, System.Collections.IList, IReadOnlyList<T>
    {
        private Dictionary<int, Pos> pos = new Dictionary<int, Pos>();
        private Node<SimpleSingleLinkedList<T>> first;
        private int count = 0;
        private int listCount = 0;
        private float itemsPerListToNumListsRatio;

        public ListInAList()
        {
            itemsPerListToNumListsRatio = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ratio">The ratio between items per inner list and the total number of inner lists. Default is 1</param>
        public ListInAList(float ratio)
        {
            itemsPerListToNumListsRatio = ratio;
        }

        public void Add(T item)
        {
            if (first == null)
            {
                first = new Node<SimpleSingleLinkedList<T>>(new SimpleSingleLinkedList<T>(item));
                pos.Add(0, new Pos(0, 0));
                listCount++;
            }
            else
            {
                if ((count + 1.0) / listCount / listCount > itemsPerListToNumListsRatio)
                {
                    var temp = first;
                    while (temp.Next != null)
                    {
                        temp = temp.Next;
                    }
                    temp.Next = new Node<SimpleSingleLinkedList<T>>(new SimpleSingleLinkedList<T>(item));
                    pos.Add(count, new Pos(listCount, 0));
                    listCount++;
                }
                else
                {
                    var temp = first;
                    int i = 0;
                    while (temp.Element.Count + 1 > itemsPerListToNumListsRatio * listCount)
                    {
                        temp = temp.Next;
                        i++;
                    }
                    temp.Element.Add(item);
                    pos.Add(count, new Pos(i, temp.Element.Count - 1));
                }
            }
            count++;
        }

        public void Clear()
        {
            first = null;
            pos.Clear();
            count = 0;
            listCount = 0;
        }

        public bool Contains(T item)
        {
            foreach(var element in this)
            {
                if (item.Equals(element))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int index)
        {
            if (index < 0 || array == null || array.Length - index < count)
                throw new IndexOutOfRangeException($"Index {index + count} outside of range {index} to {array.Length}");
            for (int i = 0; i < count; i++)
            {
                array[index + i] = GetAt(i);
            }
        }

        public int Count { get { return count; } private set { count = value; } }

        public T GetAt(int index)
        {
            var temp = first;
            for (int i = 0; i < pos[index].PosOuter; i++)
            {
                temp = temp.Next;
            }
            return temp.Element.GetAt(pos[index].PosInner);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T element)
        {
            for (int i = 0; i < count; i++)
            {
                if (GetAt(i).Equals(element))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > count)
                throw new IndexOutOfRangeException($"Index {index} outside of range 0 to {count}");
            if (first == null)
            {
                first = new Node<SimpleSingleLinkedList<T>>(new SimpleSingleLinkedList<T>(item));
                pos.Add(0, new Pos(0, 0));
                listCount++;
            }
            else
            {
                if ((count + 1.0) / listCount / listCount > itemsPerListToNumListsRatio)
                {
                    var temp = first;
                    while (temp.Next != null)
                    {
                        temp = temp.Next;
                    }
                    temp.Next = new Node<SimpleSingleLinkedList<T>>(new SimpleSingleLinkedList<T>(item));//ta bort tomma listor i removeAt
                    pos.Add(count, new Pos(listCount, 0));
                    listCount++;
                }
                else
                {
                    var temp = first;
                    int i = 0;
                    while (temp.Element.Count + 1 > itemsPerListToNumListsRatio * listCount)//fixa pos entries
                    {
                        temp = temp.Next;
                        i++;
                    }
                    temp.Element.Add(item);
                    for (int j = count; j > index; j--)
                    {
                        pos.Add(j, pos[j - 1]);
                    }

                    pos.Add(index, new Pos(i, temp.Element.Count));
                }
            }
            count++;
        }

        public bool IsReadOnly
        {
            get;//todo
        }

        public bool Remove(T item)
        {
            return false;//todo
        }

        public void RemoveAt(int index)
        {
            //todo
        }

        public T this[int index]
        {
            get
            {
                if (index > count || index < 0)
                {
                    throw new IndexOutOfRangeException($"Index {index} outside of range 0 to {count}");
                }
                return GetAt(index);
            }
            set
            {
                if (index > count || index < 0)
                {
                    throw new IndexOutOfRangeException($"Index {index} outside of range 0 to {count}");
                }
                var temp = first;
                for (int i = 0; i < pos[index].PosOuter; i++)
                {
                    temp = temp.Next;
                }
                temp.Element[pos[index].PosInner] = value;
            }
        }

        public class SimpleSingleLinkedList<E>
        {
            private Node<E> first;
            private int count = 0;

            public SimpleSingleLinkedList()
            {

            }

            public SimpleSingleLinkedList(E element)
            {
                Add(element);
            }

            public void Add(E item)
            {
                if (first == null)
                {
                    first = new Node<E>(item);
                }
                else
                {
                    Node<E> last = first;
                    while (last.Next != null)
                    {
                        last = last.Next;
                    }
                    last.Next = new Node<E>(item);
                }
                count++;
            }

            public void Insert(int index, E item)
            {
                if (index < 0 || index > Count)
                    throw new IndexOutOfRangeException($"Index {index} outside of range 0 to {count}");
                if (first == null)
                {
                    Add(item);
                }
                else
                {
                    var temp = first;
                    for (int i = 0; i < index - 1; i++)
                    {
                        temp = temp.Next;
                    }
                    var node = new Node<E>(item);
                    node.Next = temp.Next;
                    temp.Next = node;
                }
                count++;
            }

            public void RemoveAt(int index)
            {
                if (first == null || index >= count)
                    throw new IndexOutOfRangeException($"index is out of range: {index} with a count of {count} items in the collection");
                Node<E> temp = first;
                for (int i = 0; i < index - 1; i++)
                {
                    temp = temp.Next;
                }
                Node<E> next = temp.Next.Next;
                temp.Next.Next = null;
                temp.Next = next;
                count--;
            }

            public E GetAt(int index)
            {
                if (first == null || index >= count || index < 0)
                    throw new IndexOutOfRangeException($"index is out of range: {index} with a count of {count} items in the collection");
                Node<E> temp = first;
                for (int i = 0; i < index; i++)
                {
                    temp = temp.Next;
                }
                return temp.Element;
            }

            public int Count { get { return count; } private set { count = value; } }

            public E this[int index]
            {
                get
                {
                    if (index > count || index < 0)
                    {
                        throw new IndexOutOfRangeException($"Index {index} outside of range 0 to {count}");
                    }
                    return GetAt(index);
                }
                set
                {
                    if (index > count || index < 0)
                    {
                        throw new IndexOutOfRangeException($"Index {index} outside of range 0 to {count}");
                    }
                    var temp = first;
                    for (int i = 0; i < index; i++)
                    {
                        temp = temp.Next;
                    }
                    temp.Element = value;
                }
            }
        }

        class Node<E>
        {
            private E element;
            private Node<E> next;

            public Node(E e)
            {
                element = e;
            }

            public Node<E> Next { get { return next; } set { next = value; } }

            public E Element { get { return element; } set { element = value; } }
        }

        class Pos
        {
            private int posOuter;
            private int posInner;

            public Pos()
            {

            }

            public Pos(int posOuter, int posInner)
            {
                this.posOuter = posOuter;
                this.posInner = posInner;
            }

            public int PosInner
            {
                get { return posInner; }
                set { posInner = value; }
            }

            public int PosOuter
            {
                get { return posOuter; }
                set { posOuter = value; }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator
{
    public class AsyncQueue<T> where T : class
    {
        private Queue<T> queue { get; set; } = new();
        public void Enqueue(T item) => queue.Enqueue(item);
        public async Task<T> DequeueAsync()
        {
            return await Task.Run(() => ReturnItem());
        }
        private T ReturnItem()
        {
            while (true)
            {
                if (queue.Count > 0)
                    return queue.Dequeue();
            }
        }
    }
}

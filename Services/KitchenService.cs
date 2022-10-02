using Kitchen.Interfaces;
using Kitchen.Models;

namespace Kitchen.Services
{
    public class KitchenService : IKitchen
    {
        private Queue<Order> queue = new Queue<Order>();
        private Mutex mutex = new Mutex();
        Random rnd = new Random();
        private HttpClient httpClient;
        private readonly ILogger<KitchenService> _logger;
        public KitchenService(ILogger<KitchenService> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://host.docker.internal:8080/");
           // _logger.LogInformation($"Constructor start ");
            for (int i = 0; i < 10; i++)
            {
                //_logger.LogInformation($"Constructor-for  ");
                Task.Run(() => PrepareOrder());
            }
        
        }

        public void ReceiveOrder(Order order)
        {
            _logger.LogInformation($"Receive Order{order.order_id} ");
            queue.Enqueue(order);
        }

        public void PrepareOrder()
        {
            //_logger.LogInformation($"PrepareOrder ");
            while (true)
            {
               // _logger.LogInformation($"Prepare order while ");
                mutex.WaitOne();
                if (queue.Count != 0)
                {
                    //_logger.LogInformation($"prepare order if ");

                    var order = queue.Dequeue();
                    var cookingtime = Convert.ToInt32(order.max_wait * 1.3);
                    List<CookingDetails> list = new List<CookingDetails>();
                    var items = order.items;
                    for (int i = 0; i < items.Length; i++)
                    {
                        //_logger.LogInformation($"Prepare order if-for");
                        list.Add(new CookingDetails{cook_id = 1, food_id = items[i]});
                    }

                    var returnOrder = new ReturnOrder
                    {
                        order_id = order.order_id,
                        waiter_id = order.waiter_id,
                        priority = order.priority,
                        items = order.items,
                        table_id = order.table_id,
                        max_wait = order.max_wait,
                        cooking_time = cookingtime,
                        cooking_details = list
                    };
                    SendReturnOrder(returnOrder);
                }
                mutex.ReleaseMutex();
            }
        }

        public void SendReturnOrder(ReturnOrder returnOrder)
        {
            _logger.LogInformation($"send return order{returnOrder.order_id} ");
            httpClient.PostAsJsonAsync("DiningHall/Distribution", returnOrder);
        }
    }
}

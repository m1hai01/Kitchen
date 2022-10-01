namespace Kitchen.Models
{
    public class Order
    {
        public int order_id;
        public int table_id;
        public int waiter_id;
        public int[] items;
        public int priority;
        public int max_wait;
        public DateTime pick_up_timev = DateTime.Now;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList.Model
{
    class GrocItem
    {

        public String grocItem { get; set; }
        public String itemQnty { get; set; }

        public GrocItem()
        {
        }

        //Overloaded Constructor
        public GrocItem(String grocItem, String itemQnty)
        {
            this.grocItem = grocItem;
            this.itemQnty = itemQnty;
        }

        public override string ToString()
        {
            return $"     {grocItem}  -  Quantity: {itemQnty}";
        }

    }
}

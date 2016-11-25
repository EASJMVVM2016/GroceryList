using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;

namespace GroceryList.Model
{
    class obsGrocList : ObservableCollection<GrocItem>
    {

        public String GetJson()
        {
            String jsonData = JsonConvert.SerializeObject(this);
            return jsonData;
        }

    }


}

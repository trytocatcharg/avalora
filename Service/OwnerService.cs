using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OwnerService
    {
        private List<OwnerViewModel> OwnerList { get; set; }

        public OwnerService()
        {
            
            OwnerList=new List<OwnerViewModel>();
            OwnerList.Add(new OwnerViewModel
            {
                ID = 1,
                Name = "Nombre 1",
                Last1 = "Apellido 1",
                Last2 = "Apellido 2"
            });

            OwnerList.Add(new OwnerViewModel
            {
                ID = 2,
                Name = "Nombre 2",
                Last1 = "Apellido 1.1",
                Last2 = "Apellido 2.1"
            });

            OwnerList.Add(new OwnerViewModel
            {
                ID = 3,
                Name = "Nombre 3",
                Last1 = "Apellido 1.2",
                Last2 = "Apellido 2.2"
            });
        }


        public IEnumerable<OwnerViewModel> GetData()
        {
            return OwnerList.ToList();
        }


        public OwnerViewModel GetOne(int id)
        {
            return OwnerList.FirstOrDefault(e => e.ID == id);
        }
        public OwnerViewModel GetOne(int id,IEnumerable<OwnerViewModel>model )
        {
            return model.FirstOrDefault(e => e.ID == id);
        }

        public IEnumerable<OwnerViewModel> Delete(int id)
        {
            var value = OwnerList.FirstOrDefault(e => e.ID == id);
            if (value == null) throw new ArgumentNullException(nameof(value));
            OwnerList.Remove(value);
            return OwnerList;
        }
        public IEnumerable<OwnerViewModel> Edit(OwnerViewModel val)
        {
            var value = OwnerList.FirstOrDefault(e => e.ID == val.ID);
            if (value == null) throw new ArgumentNullException(nameof(value));
            OwnerList.Remove(value);
            OwnerList.Add(val);
            return OwnerList;
        }

        public object Add(OwnerViewModel objAdd)
        {
            var maxID = OwnerList.Count + 1;
            objAdd.ID = maxID;
            OwnerList.Add(objAdd);
            return OwnerList;
        }

        public void updateData(IEnumerable<OwnerViewModel> ownerViewModels)
        {
            OwnerList = ownerViewModels.ToList();
        }
    }
}

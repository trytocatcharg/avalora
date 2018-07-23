using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.BD;

namespace Service
{
    public class OwnerService
    {
        public PruebaEntities Context { get; set; }
        public IEnumerable<OwnerViewModel> GetData()
        {
            return (from c in Context.People
                                 select new OwnerViewModel
                                 {
                                     ID = c.ID,
                                     Last1 = c.Last1,
                                     Name = c.Name,
                                     Last2 = c.Last2
                                 }).ToList();
        }

        public OwnerService()
        {
            Context=new PruebaEntities();
        }

        public OwnerViewModel GetOne(int id)
        {
            var oneRow = (from c in Context.People
                where c.ID == id
                select new OwnerViewModel
                {
                    ID = c.ID,
                    Last1 = c.Last1,
                    Name = c.Name,
                    Last2 = c.Last2
                }).FirstOrDefault();

            return oneRow;
        }

        public  bool Delete(int id)
        {
            var model = Context.People.FirstOrDefault(e => e.ID == id);
            if (model == null) throw new ArgumentNullException(nameof(model));

            Context.People.Remove(model);
            Context.SaveChanges();
            return true;
        }

        public bool Edit(OwnerViewModel val)
        {
            var model = Context.People.FirstOrDefault(e => e.ID == val.ID);
            if (model == null) throw new ArgumentNullException(nameof(model));

            model.Name = val.Name;
            model.Last1 = val.Last1;
            model.Last2 = val.Last2;
            Context.SaveChanges();

            return true;
        }

        public bool Add(OwnerViewModel objAdd)
        {
            
            var newRow = new People
            {
                Name = objAdd.Name,
                Last1 = objAdd.Last1,
                Last2 = objAdd.Last2
            };
            Context.People.Add(newRow);

            Context.SaveChanges();
            return true;
        }
      
    }
}

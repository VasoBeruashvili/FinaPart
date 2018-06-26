using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using col = System.Collections;


namespace FinaPart.Utils
{
    public interface IEntity
    {
        int ID { get; set; }
        bool Deleted { get; set; }
    }

    public static class FinaContextExtensions
    {
        public static void DeleteWithRelatedObjects<TEntity>(this FinaContext context, TEntity entity, bool root = true) where TEntity : class, IEntity
        {
            var set = context.Set(entity.GetType());

            var dbentity = set.Find(entity.ID);

            if (dbentity != null)
            {
                var entry = context.Entry(dbentity);

                var children = dbentity.GetType().GetProperties()
                   .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)).ToList();

                foreach (var child in children)
                {
                    var collection = (col.IEnumerable)child.GetValue(dbentity, null);

                    List<object> relatedItems = new List<object>();

                    if(collection != null)
                    {
                        foreach (var item in collection)
                        {
                            relatedItems.Add(item);
                        }
                    }

                    foreach (var relatedEntity in relatedItems.ToList())
                    {
                        context.DeleteWithRelatedObjects((IEntity)relatedEntity, false);
                    }
                }

                context.Entry(dbentity).State = EntityState.Deleted;
                set.Remove(dbentity);
            }
        }

        public static void Save<TEntity>(this FinaContext context, TEntity entity) where TEntity : class, IEntity
        {
            if (entity.ID <= 0 && !entity.Deleted) //add
            {
                var set = context.Set(entity.GetType());

                set.Add(entity);
            }
            else
            {
                if (entity.Deleted && entity.ID >= 0) //delete
                {
                    var set2 = context.Set(entity.GetType());

                    var dbentity = set2.Find(entity.ID);

                    if (dbentity != null)
                    {
                        var entry = context.Entry(dbentity);

                        var children = dbentity.GetType().GetProperties()
                           .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)).ToList();

                        foreach (var child in children)
                        {
                            var collection = (col.IEnumerable)child.GetValue(dbentity, null);

                            List<object> relatedItems = new List<object>();

                            if (collection != null)
                            {
                                foreach (var item in collection)
                                {
                                    relatedItems.Add(item);
                                }
                            }

                            foreach (var relatedEntity in relatedItems.ToList())
                            {
                                context.DeleteWithRelatedObjects((IEntity)relatedEntity, false);
                            }
                        }
                    }

                    context.DeleteWithRelatedObjects(entity, false);
                }
                else //update
                {
                    context.Entry(entity).State = EntityState.Added;
                    context.Entry(entity).State = EntityState.Modified;

                    var set2 = context.Set(entity.GetType());

                    var dbentity = set2.Find(entity.ID);

                    if (dbentity != null)
                    {
                        var entry = context.Entry(dbentity);

                        var children = dbentity.GetType().GetProperties()
                           .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)).ToList();

                        foreach (var child in children)
                        {
                            var collection = (col.IEnumerable)child.GetValue(dbentity, null);

                            List<object> relatedItems = new List<object>();

                            if (collection != null)
                            {
                                foreach (var item in collection)
                                {
                                    relatedItems.Add(item);
                                }
                            }

                            foreach (var relatedEntity in relatedItems.ToList())
                            {
                                context.Save((IEntity)relatedEntity);
                            }
                        }

                        context.Entry(dbentity).State = EntityState.Modified;
                    }
                }
            }         
        }
    }
}
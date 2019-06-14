namespace ComicBookShopCore.Data.Interfaces
{
    public interface IEditable<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
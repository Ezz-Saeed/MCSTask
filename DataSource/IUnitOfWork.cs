using MCSExam.Models;

namespace MCSExam.DataSource
{
    public interface IUnitOfWork
    {
        IRepository _repository { get; }
    }
}

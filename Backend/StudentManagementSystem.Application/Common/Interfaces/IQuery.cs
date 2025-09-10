using MediatR;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

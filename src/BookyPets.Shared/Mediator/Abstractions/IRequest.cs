namespace BookyPets.Shared.Mediator.Abstractions;

public interface IBaseRequest { }

public interface IRequest : IBaseRequest { }

public interface IRequest<TResponse> : IBaseRequest { }

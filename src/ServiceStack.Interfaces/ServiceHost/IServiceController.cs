using System;
using System.Collections.Generic;
using ServiceStack.Configuration;

namespace ServiceStack.ServiceHost
{
    public delegate object ResponseBinderFn(object service, object request, object response);
    public delegate IServiceResult AsyncResponseBinderFn(object service, object request, object response, Action<IServiceResult> callback);

    public delegate IServiceResult ExecuteServiceFn(IRequestContext requestContext, object request, Action<IServiceResult> callback);

    public delegate void ExecuteOneWayServiceFn(IRequestContext requestContext, object request);

    /// <summary>
    /// Responsible for executing the operation within the specified context.
    /// </summary>
    /// <value>The operation types.</value>
    public interface IServiceController
    {
        /// <summary>
        /// Maps each request type to its appropriate service type.
        /// </summary>
        Dictionary<Type, Type> RequestServiceTypeMap { get; set; }

        /// <summary>
        /// Maps each response type to its appropriate service type.
        /// </summary>
        Dictionary<Type, Type> ResponseServiceTypeMap { get; set; }

        Dictionary<Type, Func<IHttpRequest, object>> RequestTypeFactoryMap { get; set; }

        Dictionary<Type, ResponseBinderFn> ResponseBinders { get; set; }

        /// <summary>
        /// Returns a list of asynchronous response binders.
        /// The async response binder should return null if the response is not converted.
        /// </summary>
        List<AsyncResponseBinderFn> AsyncResponseBinders { get; set; }

        HashSet<Type> ServiceTypes { get; }

        /// <summary>
        /// Returns a list of registered operation types (ie request & response types).
        /// </summary>
        IList<Type> OperationTypes { get; }

        /// <summary>
        /// Returns a list of all request types
        /// </summary>
        IList<Type> RequestTypes { get; }

        /// <summary>
        /// Executes the DTO request under the supplied requestContext.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        [Obsolete]
        object Execute(object request, IRequestContext requestContext = null);

        IServiceResult ExecuteAsync(object request, Action<IServiceResult> callback, IRequestContext requestContext = null);

        void ExecuteOneWay(object request, IRequestContext requestContext = null);

        /// <summary>
        /// Registers multiple services.
        /// </summary>
        /// <param name="serviceFactory">The factory which creates the service instances</param>
        /// <param name="serviceTypes">The type of the service</param>
        void RegisterServices(ITypeFactory serviceFactory, IEnumerable<Type> serviceTypes);

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <param name="serviceFactory">The factory which creates the service instance</param>
        /// <param name="serviceType">The type of the service</param>
        void RegisterService(ITypeFactory serviceFactory, Type serviceType);

        ExecuteServiceFn GetService(Type requestType);

        ExecuteOneWayServiceFn GetOneWayService(Type requestType);
    }
}
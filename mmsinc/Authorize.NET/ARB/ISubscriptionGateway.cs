using AuthorizeNet.APICore;

// ReSharper disable CheckNamespace
namespace AuthorizeNet
{
// ReSharper restore CheckNamespace
    public interface ISubscriptionGateway {
        bool CancelSubscription(string subscriptionID);
        ISubscriptionRequest CreateSubscription(ISubscriptionRequest subscription);
        ARBSubscriptionStatusEnum GetSubscriptionStatus(string subscriptionID);
        bool UpdateSubscription(ISubscriptionRequest subscription);
    }
}

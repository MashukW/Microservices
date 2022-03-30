namespace Shared.Message;

public static class MessageConstants
{
    public static class Azure
    {
        public static class Topics
        {
            public const string CheckoutOrder = "checkout-order-topic"; // checkout-order-subscription
            public const string OrderPaymentRequest = "order-payment-request-topic"; // order-payment-request-subscription
            public const string OrderPaymentSuccess = "order-payment-success-topic"; // order-payment-success-update-status-subscription -> order-payment-success-send-email-subscription
        }

        public static class Subscriptions
        {
            public const string CheckoutOrder = "checkout-order-subscription";
            public const string OrderPaymentRequest = "order-payment-request-subscription";
            public const string OrderPaymentSuccessUpdateStatus = "order-payment-success-update-status-subscription";
            public const string OrderPaymentSuccessSendEmail = "order-payment-success-send-email-subscription";
        }
    }

    public static class RabbitMq
    {
        public static class Exchanges
        {
            public const string CheckoutDirect = "checkout-direct-exchange";
            public const string CheckoutFanout = "checkout-fanout-exchange";
        }

        public static class Queues
        {
            public const string CheckoutOrder = "checkout-order-queue";
            public const string OrderPaymentRequest = "order-payment-request-queue";
            public const string OrderPaymentSuccessUpdateStatus = "order-payment-success-update-status-queue";
            public const string OrderPaymentSuccessSendEmail = "order-payment-success-send-email-queue";
        }

        public static class RoutingKeys
        {
            public const string CheckoutOrder = "checkout-order-rk";
            public const string OrderPaymentRequest = "order-payment-request-rk";
        }
    }
}

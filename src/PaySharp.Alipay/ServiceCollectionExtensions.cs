﻿#if NETCOREAPP3_1
using System;
using Microsoft.Extensions.Configuration;
using PaySharp.Alipay;
using PaySharp.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IGateways UseAlipay(this IGateways gateways, Action<Merchant> action)
        {
            if (action != null)
            {
                var merchant = new Merchant();
                action(merchant);
                gateways.Add(new AlipayGateway(merchant));
            }

            return gateways;
        }

        public static IGateways UseAlipay(this IGateways gateways, IConfiguration configuration)
        {
            var merchants = configuration.GetSection("PaySharp:Alipays").Get<Merchant[]>();
            if (merchants != null)
            {
                for (var i = 0; i < merchants.Length; i++)
                {
                    var alipayGateway = new AlipayGateway(merchants[i]);
                    var gatewayUrl = configuration.GetSection($"PaySharp:Alipays:{i}:GatewayUrl").Value;
                    if (!string.IsNullOrEmpty(gatewayUrl))
                    {
                        alipayGateway.GatewayUrl = gatewayUrl;
                    }

                    gateways.Add(alipayGateway);
                }
            }

            return gateways;
        }
    }
}

#endif

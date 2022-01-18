# Generated by the gRPC Python protocol compiler plugin. DO NOT EDIT!
"""Client and server classes corresponding to protobuf-defined services."""
import grpc

import price_pb2 as price__pb2


class PricerStub(object):
    """Missing associated documentation comment in .proto file."""

    def __init__(self, channel):
        """Constructor.

        Args:
            channel: A grpc.Channel.
        """
        self.GetPrice = channel.unary_unary(
                '/price.Pricer/GetPrice',
                request_serializer=price__pb2.PriceRequest.SerializeToString,
                response_deserializer=price__pb2.PriceReply.FromString,
                )


class PricerServicer(object):
    """Missing associated documentation comment in .proto file."""

    def GetPrice(self, request, context):
        """Sends a greeting
        """
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')


def add_PricerServicer_to_server(servicer, server):
    rpc_method_handlers = {
            'GetPrice': grpc.unary_unary_rpc_method_handler(
                    servicer.GetPrice,
                    request_deserializer=price__pb2.PriceRequest.FromString,
                    response_serializer=price__pb2.PriceReply.SerializeToString,
            ),
    }
    generic_handler = grpc.method_handlers_generic_handler(
            'price.Pricer', rpc_method_handlers)
    server.add_generic_rpc_handlers((generic_handler,))


 # This class is part of an EXPERIMENTAL API.
class Pricer(object):
    """Missing associated documentation comment in .proto file."""

    @staticmethod
    def GetPrice(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/price.Pricer/GetPrice',
            price__pb2.PriceRequest.SerializeToString,
            price__pb2.PriceReply.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

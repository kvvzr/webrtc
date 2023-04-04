/*
 *  Copyright 2019 pixiv Inc. All Rights Reserved.
 *
 *  Use of this source code is governed by a license that can be
 *  found in the LICENSE.pixiv file in the root of the source tree.
 */

using Pixiv.Cricket;
using Pixiv.Rtc;
using System;
using System.Runtime.InteropServices;

namespace Pixiv.Webrtc
{
    public enum SdpSemantics
    {
        PlanB,
        UnifiedPlan
    }

    public interface IDisposablePeerConnectionFactoryInterface :
        IPeerConnectionFactoryInterface, Rtc.IDisposable
    {
    }

    public interface IDisposablePeerConnectionInterface :
        IPeerConnectionInterface, Rtc.IDisposable
    {
    }

    public interface IDisposablePeerConnectionObserver :
        IPeerConnectionObserver, Rtc.IDisposable
    {
    }

    public interface IManagedPeerConnectionObserver
    {
        void OnSignalingChange(PeerConnectionInterface.SignalingState newState);
        void OnAddStream(DisposableMediaStreamInterface stream);
        void OnRemoveStream(DisposableMediaStreamInterface stream);
        void OnDataChannel(DisposableDataChannelInterface dataChannel);
        void OnRenegotiationNeeded();
        void OnIceConnectionChange(PeerConnectionInterface.IceConnectionState newState);
        void OnStandardizedIceConnectionChange(PeerConnectionInterface.IceConnectionState newState);
        void OnConnectionChange();
        void OnIceGatheringChange(PeerConnectionInterface.IceGatheringState newState);
        void OnIceCandidate(IceCandidateInterface candidate);
        void OnIceCandidatesRemoved(DisposableCandidate[] candidates);
        void OnIceConnectionReceivingChange(bool receiving);
        void OnAddTrack(DisposableRtpReceiverInterface receiver, DisposableMediaStreamInterface[] streams);
        void OnTrack(DisposableRtpTransceiverInterface transceiver);
        void OnRemoveTrack(DisposableRtpReceiverInterface receiver);
        void OnInterestingUsage(int usagePattern);
    }

    public interface IPeerConnectionFactoryInterface
    {
        IntPtr Ptr { get; }
    }

    public interface IPeerConnectionInterface
    {
        IntPtr Ptr { get; }
    }

    public interface IPeerConnectionObserver
    {
        IntPtr Ptr { get; }
    }

    public sealed class DisposablePeerConnectionFactoryInterface :
        DisposablePtr, IDisposablePeerConnectionFactoryInterface
    {
        IntPtr IPeerConnectionFactoryInterface.Ptr => Ptr;

        public DisposablePeerConnectionFactoryInterface(IntPtr ptr)
        {
            Ptr = ptr;
        }

        private protected override void FreePtr()
        {
            Interop.PeerConnectionFactoryInterface.Release(Ptr);
        }
    }

    public sealed class DisposablePeerConnectionInterface :
        DisposablePtr, IDisposablePeerConnectionInterface
    {
        IntPtr IPeerConnectionInterface.Ptr => Ptr;

        public DisposablePeerConnectionInterface(IntPtr ptr)
        {
            Ptr = ptr;
        }

        private protected override void FreePtr()
        {
            Interop.PeerConnectionInterface.Release(Ptr);
        }
    }

    public sealed class DisposablePeerConnectionObserver :
        DisposablePtr, IDisposablePeerConnectionObserver
    {
        IntPtr IPeerConnectionObserver.Ptr => Ptr;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DestructionHandler(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SignalingChangeHandler(
            IntPtr context,
            PeerConnectionInterface.SignalingState newState
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void AddStreamHandler(
            IntPtr context,
            IntPtr stream
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void RemoveStreamHandler(
            IntPtr context,
            IntPtr stream
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DataChannelHandler(
            IntPtr context,
            IntPtr dataChannel
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void RenegotiationNeededHandler(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void IceConnectionChangeHandler(
            IntPtr context,
            PeerConnectionInterface.IceConnectionState newState
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void StandardizedIceConnectionChangeHandler(
            IntPtr context,
            PeerConnectionInterface.IceConnectionState newState
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ConnectionChangeHandler(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void IceGatheringChangeHandler(
            IntPtr context,
            PeerConnectionInterface.IceGatheringState newState
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void IceCandidateHandler(
            IntPtr context,
            IntPtr candidate
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void IceCandidatesRemovedHandler(
            IntPtr context,
            IntPtr data,
            UIntPtr size
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void IceConnectionReceivingChangeHandler(
            IntPtr context,
            bool receiving
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void AddTrackHandler(
            IntPtr context,
            IntPtr receiver,
            IntPtr data,
            UIntPtr size
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TrackHandler(
            IntPtr context,
            IntPtr transceiver
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void RemoveTrackHandler(
            IntPtr context,
            IntPtr receiver
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void InterestingUsageHandler(
            IntPtr context,
            int usagePattern
        );

        private static readonly FunctionPtrArray s_functions = new FunctionPtrArray(
            (DestructionHandler)OnDestruction,
            (SignalingChangeHandler)OnSignalingChange,
            (AddStreamHandler)OnAddStream,
            (RemoveStreamHandler)OnRemoveStream,
            (DataChannelHandler)OnDataChannel,
            (RenegotiationNeededHandler)OnRenegotiationNeeded,
            (IceConnectionChangeHandler)OnIceConnectionChange,
            (StandardizedIceConnectionChangeHandler)OnStandardizedIceConnectionChange,
            (ConnectionChangeHandler)OnConnectionChange,
            (IceGatheringChangeHandler)OnIceGatheringChange,
            (IceCandidateHandler)OnIceCandidate,
            (IceCandidatesRemovedHandler)OnIceCandidatesRemoved,
            (IceConnectionReceivingChangeHandler)OnIceConnectionReceivingChange,
            (AddTrackHandler)OnAddTrack,
            (TrackHandler)OnTrack,
            (RemoveTrackHandler)OnRemoveTrack,
            (InterestingUsageHandler)OnInterestingUsage
        );

        private static IManagedPeerConnectionObserver GetContextTarget(
            IntPtr context)
        {
            return (IManagedPeerConnectionObserver)((GCHandle)context).Target;
        }

        [MonoPInvokeCallback(typeof(DestructionHandler))]
        private static void OnDestruction(IntPtr context)
        {
            ((GCHandle)context).Free();
        }

        [MonoPInvokeCallback(typeof(SignalingChangeHandler))]
        private static void OnSignalingChange(
            IntPtr context,
            PeerConnectionInterface.SignalingState newState)
        {
            GetContextTarget(context).OnSignalingChange(newState);
        }

        [MonoPInvokeCallback(typeof(AddStreamHandler))]
        private static void OnAddStream(IntPtr context, IntPtr stream)
        {
            GetContextTarget(context).OnAddStream(
                new DisposableMediaStreamInterface(stream)
            );
        }

        [MonoPInvokeCallback(typeof(RemoveStreamHandler))]
        private static void OnRemoveStream(IntPtr context, IntPtr stream)
        {
            GetContextTarget(context).OnRemoveStream(
                new DisposableMediaStreamInterface(stream)
            );
        }

        [MonoPInvokeCallback(typeof(DataChannelHandler))]
        private static void OnDataChannel(IntPtr context, IntPtr dataChannel)
        {
            GetContextTarget(context).OnDataChannel(
                new DisposableDataChannelInterface(dataChannel)
            );
        }

        [MonoPInvokeCallback(typeof(RenegotiationNeededHandler))]
        private static void OnRenegotiationNeeded(IntPtr context)
        {
            GetContextTarget(context).OnRenegotiationNeeded();
        }

        [MonoPInvokeCallback(typeof(IceConnectionChangeHandler))]
        private static void OnIceConnectionChange(
            IntPtr context,
            PeerConnectionInterface.IceConnectionState newState)
        {
            GetContextTarget(context).OnIceConnectionChange(newState);
        }

        [MonoPInvokeCallback(typeof(StandardizedIceConnectionChangeHandler))]
        private static void OnStandardizedIceConnectionChange(
            IntPtr context,
            PeerConnectionInterface.IceConnectionState newState)
        {
            GetContextTarget(context).OnStandardizedIceConnectionChange(
                newState
            );
        }

        [MonoPInvokeCallback(typeof(ConnectionChangeHandler))]
        private static void OnConnectionChange(IntPtr context)
        {
            GetContextTarget(context).OnConnectionChange();
        }

        [MonoPInvokeCallback(typeof(IceGatheringChangeHandler))]
        private static void OnIceGatheringChange(
            IntPtr context,
            PeerConnectionInterface.IceGatheringState newState)
        {
            GetContextTarget(context).OnIceGatheringChange(newState);
        }

        [MonoPInvokeCallback(typeof(IceCandidateHandler))]
        private static void OnIceCandidate(IntPtr context, IntPtr candidate)
        {
            GetContextTarget(context).OnIceCandidate(
                new IceCandidateInterface(candidate)
            );
        }

        [MonoPInvokeCallback(typeof(IceCandidatesRemovedHandler))]
        private static void OnIceCandidatesRemoved(
            IntPtr context,
            IntPtr data,
            UIntPtr nativeSize)
        {
            var size = (ulong)nativeSize;
            var sizeOfIntPtr = Marshal.SizeOf<IntPtr>();
            var candidates = new DisposableCandidate[size];

            for (ulong index = 0; index < size; index++)
            {
                var ptr = Marshal.ReadIntPtr(data);
                candidates[index] = new DisposableCandidate(ptr);
                data += sizeOfIntPtr;
            }

            GetContextTarget(context).OnIceCandidatesRemoved(candidates);
        }

        [MonoPInvokeCallback(typeof(IceConnectionReceivingChangeHandler))]
        private static void OnIceConnectionReceivingChange(
            IntPtr context,
            bool receiving)
        {
            GetContextTarget(context).OnIceConnectionReceivingChange(
                receiving
            );
        }

        [MonoPInvokeCallback(typeof(AddTrackHandler))]
        private static void OnAddTrack(
            IntPtr context,
            IntPtr receiver,
            IntPtr data,
            UIntPtr nativeSize)
        {
            var size = (ulong)nativeSize;
            var sizeOfIntPtr = Marshal.SizeOf<IntPtr>();
            var streams = new DisposableMediaStreamInterface[size];

            for (ulong index = 0; index < size; index++)
            {
                var ptr = Marshal.ReadIntPtr(data);
                streams[index] = new DisposableMediaStreamInterface(ptr);
                data += sizeOfIntPtr;
            }

            GetContextTarget(context).OnAddTrack(
                new DisposableRtpReceiverInterface(receiver),
                streams
            );
        }

        [MonoPInvokeCallback(typeof(TrackHandler))]
        private static void OnTrack(IntPtr context, IntPtr transceiver)
        {
            GetContextTarget(context).OnTrack(
                new DisposableRtpTransceiverInterface(transceiver)
            );
        }

        [MonoPInvokeCallback(typeof(RemoveTrackHandler))]
        private static void OnRemoveTrack(IntPtr context, IntPtr receiver)
        {
            GetContextTarget(context).OnRemoveTrack(
                new DisposableRtpReceiverInterface(receiver)
            );
        }

        [MonoPInvokeCallback(typeof(InterestingUsageHandler))]
        private static void OnInterestingUsage(IntPtr context, int usagePattern)
        {
            GetContextTarget(context).OnInterestingUsage(usagePattern);
        }

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr webrtcNewPeerConnectionObserver(
            IntPtr context,
            IntPtr functions
        );

        public DisposablePeerConnectionObserver(
            IManagedPeerConnectionObserver managed)
        {
            Ptr = webrtcNewPeerConnectionObserver(
                (IntPtr)GCHandle.Alloc(managed),
                s_functions.Ptr
            );
        }

        private protected override void FreePtr()
        {
            Interop.PeerConnectionObserver.Delete(Ptr);
        }
    }

    public sealed class PeerConnectionDependencies
    {
        public IPeerConnectionObserver Observer { get; set; }

        public IDisposablePortAllocator Allocator { get; set; }

        public IDisposableAsyncResolverFactory AsyncResolverFactory
        { get; set; }

        public IDisposableRtcCertificateGeneratorInterface CertGenerator
        { get; set; }

        public IDisposableSslCertificateVerifier TlsCertVerifier { get; set; }

        public IDisposableVideoBitrateAllocatorFactory VideoBitrateAllocatorFactory
        { get; set; }
    }

    public static class PeerConnectionFactoryInterfaceExtension
    {
        [Flags]
        private enum WebrtcRTCConfigurationFlags
        {
            Dscp = 1 << 1,
            CpuAdaptation = 1 << 2,
            SuspendBelowMinBitrate = 1 << 3,
            PrerendererSmoothing = 1 << 4,
            ExperimentCpuLoadEstimator = 1 << 5,
            OverrideEnableDtlsSrtp = 1 << 6,
            EnableDtlsSrtp = 1 << 7,
        }

        private struct WebrtcIceServer
        {
            public IntPtr Uri;
            public IntPtr Urls;
            public UIntPtr UrlsSize;
            public IntPtr Username;
            public IntPtr Password;
            public PeerConnectionInterface.TlsCertPolicy TlsCertPolicy;
            public IntPtr Hostname;
            public IntPtr TlsAlpnProtocols;
            public UIntPtr TlsAlpnProtocolsSize;
            public IntPtr TlsEllipticCurves;
            public UIntPtr TlsEllipticCurvesSize;
        }

        private struct WebrtcPeerConnectionDependencies
        {
            public IntPtr Observer;
            public IntPtr Allocator;
            public IntPtr AsyncResolverFactory;
            public IntPtr CertGenerator;
            public IntPtr TlsCertVerifier;
            public IntPtr VideoBitrateAllocatorFactory;
        }

        private struct WebrtcRTCConfiguration
        {
            [MarshalAs(UnmanagedType.I4)]
            public WebrtcRTCConfigurationFlags Flags;
            public int AudioRtcpReportIntervalMs;
            public int VideoRtcpReportIntervalMs;
            public IntPtr Servers;
            public UIntPtr ServersSize;
            public PeerConnectionInterface.IceTransportsType Type;
            public PeerConnectionInterface.BundlePolicy BundlePolicy;
            public PeerConnectionInterface.RtcpMuxPolicy RtcpMuxPolicy;
            public IntPtr Certificates;
            public UIntPtr CertificatesSize;
            public int IceCandidatePoolSize;
            public SdpSemantics SdpSemantics;
        }

        private static IntPtr StringArrayToHGlobalAnsiArrayPtr(string[] array)
        {
            var sizeOfIntPtr = Marshal.SizeOf<IntPtr>();
            var ptr = Marshal.AllocHGlobal(array.Length * sizeOfIntPtr);

            for (var index = 0; index < array.Length; index++)
            {
                Marshal.WriteIntPtr(
                    ptr,
                    index * sizeOfIntPtr,
                    Marshal.StringToHGlobalAnsi(array[index])
                );
            }

            return ptr;
        }

        private static void FreeHGlobalAnsiArrayPtr(IntPtr ptr, string[] array)
        {
            var sizeOfIntPtr = Marshal.SizeOf<IntPtr>();

            for (var index = 0; index < array.Length; index++)
            {
                Marshal.FreeHGlobal(Marshal.ReadIntPtr(
                    ptr,
                    index * sizeOfIntPtr
                ));
            }

            Marshal.FreeHGlobal(ptr);
        }

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr webrtcPeerConnectionFactoryInterfaceCreatePeerConnection(
            IntPtr factory,
            in WebrtcRTCConfiguration configuration,
            in WebrtcPeerConnectionDependencies dependencies);

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr webrtcPeerConnectionFactoryInterfaceCreateAudioTrack(
            IntPtr factory,
            string label,
            IntPtr source);

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr webrtcPeerConnectionFactoryInterfaceCreateVideoTrack(
            IntPtr factory,
            string label,
            IntPtr source);

        public static DisposablePeerConnectionInterface CreatePeerConnection(
            this IPeerConnectionFactoryInterface factory,
            PeerConnectionInterface.RtcConfiguration configuration,
            PeerConnectionDependencies dependencies)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (configuration.Servers == null)
            {
                throw new ArgumentException("Servers property is null", nameof(configuration));
            }

            if (configuration.Certificates == null)
            {
                throw new ArgumentException("Certificates property is null", nameof(configuration));
            }

            if (dependencies == null)
            {
                throw new ArgumentNullException(nameof(dependencies));
            }

            var servers = new WebrtcIceServer[configuration.Servers.LongLength];
            var sizeOfPtr = Marshal.SizeOf<IntPtr>();
            var unmanagedConfiguration = new WebrtcRTCConfiguration();
            var unmanagedDependencies = new WebrtcPeerConnectionDependencies();

            if (configuration.Dscp)
            {
                unmanagedConfiguration.Flags |= WebrtcRTCConfigurationFlags.Dscp;
            }

            if (configuration.CpuAdaptation)
            {
                unmanagedConfiguration.Flags |= WebrtcRTCConfigurationFlags.CpuAdaptation;
            }

            if (configuration.SuspendBelowMinBitrate)
            {
                unmanagedConfiguration.Flags |= WebrtcRTCConfigurationFlags.SuspendBelowMinBitrate;
            }

            if (configuration.PrerendererSmoothing)
            {
                unmanagedConfiguration.Flags |= WebrtcRTCConfigurationFlags.PrerendererSmoothing;
            }

            if (configuration.ExperimentCpuLoadEstimator)
            {
                unmanagedConfiguration.Flags |= WebrtcRTCConfigurationFlags.ExperimentCpuLoadEstimator;
            }

            if (configuration.EnableDtlsSrtp.HasValue)
            {
                unmanagedConfiguration.Flags |= WebrtcRTCConfigurationFlags.OverrideEnableDtlsSrtp;

                if (configuration.EnableDtlsSrtp.Value)
                {
                    unmanagedConfiguration.Flags |= WebrtcRTCConfigurationFlags.EnableDtlsSrtp;
                }
            }

            unmanagedConfiguration.AudioRtcpReportIntervalMs = configuration.AudioRtcpReportIntervalMs;
            unmanagedConfiguration.VideoRtcpReportIntervalMs = configuration.VideoRtcpReportIntervalMs;
            unmanagedConfiguration.ServersSize = new UIntPtr((uint)configuration.Servers.Length);
            unmanagedConfiguration.Type = configuration.Type;
            unmanagedConfiguration.BundlePolicy = configuration.BundlePolicy;
            unmanagedConfiguration.RtcpMuxPolicy = configuration.RtcpMuxPolicy;
            unmanagedConfiguration.CertificatesSize = new UIntPtr((uint)configuration.Certificates.Length);
            unmanagedConfiguration.IceCandidatePoolSize = configuration.IceCandidatePoolSize;
            unmanagedConfiguration.SdpSemantics = configuration.SdpSemantics;

            unmanagedDependencies.Observer = dependencies.Observer.Ptr;

            var serversHandle = GCHandle.Alloc(servers, GCHandleType.Pinned);

            var certificatesCursor = Marshal.AllocHGlobal(sizeOfPtr * configuration.Certificates.Length);
            unmanagedConfiguration.Certificates = certificatesCursor;

            try
            {
                for (var index = 0; index < configuration.Servers.Length; index++)
                {
                    var server = configuration.Servers[index];
                    ref var unmanagedServer = ref servers[index];

                    if (server.Urls == null)
                    {
                        throw new ArgumentException("Urls property of an element of Server property is null", nameof(configuration));
                    }

                    if (server.TlsAlpnProtocols == null)
                    {
                        throw new ArgumentException("TlsAlpnProtocols property of an element of Server property is null", nameof(configuration));
                    }

                    unmanagedServer.UrlsSize = new UIntPtr((ulong)server.Urls.LongLength);
                    unmanagedServer.TlsCertPolicy = server.TlsCertPolicy;
                    unmanagedServer.TlsAlpnProtocolsSize = new UIntPtr((uint)server.TlsAlpnProtocols.Length);
                    unmanagedServer.TlsEllipticCurvesSize = new UIntPtr((uint)server.TlsEllipticCurves.Length);

                    unmanagedServer.Uri = Marshal.StringToHGlobalAnsi(server.Uri);
                    unmanagedServer.Urls = StringArrayToHGlobalAnsiArrayPtr(server.Urls);
                    unmanagedServer.Username = Marshal.StringToHGlobalAnsi(server.Username);
                    unmanagedServer.Password = Marshal.StringToHGlobalAnsi(server.Password);
                    unmanagedServer.Hostname = Marshal.StringToHGlobalAnsi(server.Hostname);
                    unmanagedServer.TlsAlpnProtocols = StringArrayToHGlobalAnsiArrayPtr(server.TlsAlpnProtocols);
                    unmanagedServer.TlsEllipticCurves = StringArrayToHGlobalAnsiArrayPtr(server.TlsEllipticCurves);
                }

                if (dependencies.Allocator != null)
                {
                    unmanagedDependencies.Allocator = dependencies.Allocator.Ptr;
                    dependencies.Allocator.ReleasePtr();
                }

                if (dependencies.AsyncResolverFactory != null)
                {
                    unmanagedDependencies.AsyncResolverFactory = dependencies.AsyncResolverFactory.Ptr;
                    dependencies.AsyncResolverFactory.ReleasePtr();
                }

                if (dependencies.CertGenerator != null)
                {
                    unmanagedDependencies.CertGenerator = dependencies.CertGenerator.Ptr;
                    dependencies.CertGenerator.ReleasePtr();
                }

                if (dependencies.TlsCertVerifier != null)
                {
                    unmanagedDependencies.TlsCertVerifier = dependencies.TlsCertVerifier.Ptr;
                    dependencies.TlsCertVerifier.ReleasePtr();
                }

                if (dependencies.VideoBitrateAllocatorFactory != null)
                {
                    unmanagedDependencies.VideoBitrateAllocatorFactory = dependencies.VideoBitrateAllocatorFactory.Ptr;
                    dependencies.VideoBitrateAllocatorFactory.ReleasePtr();
                }

                unmanagedConfiguration.Servers = serversHandle.AddrOfPinnedObject();

                foreach (var certificate in configuration.Certificates)
                {
                    if (certificate == null)
                    {
                        throw new ArgumentException("An element of Certificates property is null", nameof(configuration));
                    }

                    Marshal.WriteIntPtr(certificatesCursor, certificate.Ptr);
                    certificatesCursor += sizeOfPtr;
                }

                var connection = webrtcPeerConnectionFactoryInterfaceCreatePeerConnection(
                    factory.Ptr,
                    unmanagedConfiguration,
                    unmanagedDependencies
                );

                GC.KeepAlive(factory);
                GC.KeepAlive(configuration);

                return new DisposablePeerConnectionInterface(connection);
            }
            finally
            {
                for (var index = 0; index < configuration.Servers.Length; index++)
                {
                    var server = configuration.Servers[index];
                    ref var unmanagedServer = ref servers[index];

                    if (unmanagedServer.TlsEllipticCurves != IntPtr.Zero)
                    {
                        FreeHGlobalAnsiArrayPtr(
                            unmanagedServer.TlsEllipticCurves,
                            server.TlsEllipticCurves
                        );
                    }

                    if (unmanagedServer.TlsAlpnProtocols != IntPtr.Zero)
                    {
                        FreeHGlobalAnsiArrayPtr(
                            unmanagedServer.TlsAlpnProtocols,
                            server.TlsAlpnProtocols
                        );
                    }

                    if (unmanagedServer.Urls != IntPtr.Zero)
                    {
                        FreeHGlobalAnsiArrayPtr(
                            unmanagedServer.Urls,
                            server.Urls
                        );
                    }

                    if (unmanagedServer.Hostname != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(unmanagedServer.Hostname);
                    }

                    if (unmanagedServer.Password != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(unmanagedServer.Password);
                    }

                    if (unmanagedServer.Username != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(unmanagedServer.Username);
                    }

                    if (unmanagedServer.Uri != null)
                    {
                        Marshal.FreeHGlobal(unmanagedServer.Uri);
                    }
                }

                Marshal.FreeHGlobal(unmanagedConfiguration.Certificates);
                serversHandle.Free();
            }
        }

        public static DisposableAudioTrackInterface CreateAudioTrack(
            this IPeerConnectionFactoryInterface factory,
            string label,
            IAudioSourceInterface source)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var track = webrtcPeerConnectionFactoryInterfaceCreateAudioTrack(
                factory.Ptr,
                label,
                source.Ptr
            );

            GC.KeepAlive(factory);
            GC.KeepAlive(source);

            return new DisposableAudioTrackInterface(
                Interop.AudioTrackInterface.ToWebrtcMediaStreamTrackInterface(
                    track
                )
            );
        }

        public static DisposableVideoTrackInterface CreateVideoTrack(
            this IPeerConnectionFactoryInterface factory,
            string label,
            IVideoTrackSourceInterface source)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var track = webrtcPeerConnectionFactoryInterfaceCreateVideoTrack(
                factory.Ptr,
                label,
                source.Ptr
            );

            GC.KeepAlive(factory);
            GC.KeepAlive(source);

            return new DisposableVideoTrackInterface(
                Interop.VideoTrackInterface.ToWebrtcMediaStreamTrackInterface(
                    track
                )
            );
        }
    }

    public static class PeerConnectionInterface
    {
        public enum SignalingState
        {
            Stable,
            HaveLocalOffer,
            HaveLocalPrAnswer,
            HaveRemoteOffer,
            HaveRemotePrAnswer,
            Closed,
        }

        public enum IceGatheringState
        {
            New,
            Gathering,
            Complete
        }

        public enum IceConnectionState
        {
            New,
            Checking,
            Connected,
            Completed,
            Failed,
            Disconnected,
            Closed,
            Max,
        }

        public enum IceTransportsType
        {
            None,
            Relay,
            NoHost,
            All
        }

        public enum BundlePolicy
        {
            Balanced,
            MaxBundle,
            MaxCompat
        }

        public enum RtcpMuxPolicy
        {
            Negotiate,
            Require,
        }

        public enum TlsCertPolicy
        {
            Secure,
            InsecureNoCheck
        }

        public sealed class IceServer
        {
            public string Uri { get; set; }
            public string[] Urls { get; set; } = new string[0];
            public string Username { get; set; }
            public string Password { get; set; }
            public TlsCertPolicy TlsCertPolicy { get; set; } =
                TlsCertPolicy.Secure;
            public string Hostname { get; set; }
            public string[] TlsAlpnProtocols { get; set; } = new string[0];
            public string[] TlsEllipticCurves { get; set; } = new string[0];
        }

        public sealed class RtcConfiguration
        {
            public bool Dscp { get; set; }
            public bool CpuAdaptation { get; set; }
            public bool SuspendBelowMinBitrate { get; set; }
            public bool PrerendererSmoothing { get; set; }
            public bool ExperimentCpuLoadEstimator { get; set; }
            public int AudioRtcpReportIntervalMs { get; set; } = 5000;
            public int VideoRtcpReportIntervalMs { get; set; } = 1000;
            public IceServer[] Servers { get; set; } = new IceServer[0];
            public IceTransportsType Type { get; set; } = IceTransportsType.All;
            public BundlePolicy BundlePolicy { get; set; } =
                BundlePolicy.Balanced;
            public RtcpMuxPolicy RtcpMuxPolicy { get; set; } =
                RtcpMuxPolicy.Require;
            public IRtcCertificate[] Certificates { get; set; } =
                new IRtcCertificate[0];
            public int IceCandidatePoolSize { get; set; } = 0;
            public SdpSemantics SdpSemantics;
            public bool? EnableDtlsSrtp { get; set; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public sealed class RtcOfferAnswerOptions
        {
            public const int Undefined = -1;
            public const int MaxOfferToReceiveMedia = 1;
            public const int OfferToReceiveMediaTrue = 1;

            public int OfferToReceiveVideo { get; set; } = Undefined;
            public int OfferToReceiveAudio { get; set; } = Undefined;

            [field: MarshalAs(UnmanagedType.I1)]
            public bool VoiceActivityDetection { get; set; } = true;

            [field: MarshalAs(UnmanagedType.I1)]
            public bool IceRestart { get; set; } = false;

            [field: MarshalAs(UnmanagedType.I1)]
            public bool UseRtpMux { get; set; } = true;

            [field: MarshalAs(UnmanagedType.I1)]
            public bool RawPacketizationForVideo { get; set; } = false;

            public int NumSimulcastLayers { get; set; } = 1;

            [field: MarshalAs(UnmanagedType.I1)]
            public bool UseObsoleteSctpSdp = false;
        }
    }

    public static class PeerConnectionInterfaceExtension
    {
        private readonly struct WebrtcAddTrackResult
        {
#pragma warning disable 0649
            public readonly IntPtr Error;
            public readonly IntPtr Value;
#pragma warning restore 0649
        }

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void webrtcDeleteRtpSenderInterfaces(
            IntPtr interfaces);

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool webrtcPeerConnectionInterfaceAddIceCandidate(
            IntPtr connection,
            IntPtr candidate
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern WebrtcAddTrackResult webrtcPeerConnectionInterfaceAddTrack(
            IntPtr connection,
            IntPtr track,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] string[] data,
            UIntPtr size
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void webrtcPeerConnectionInterfaceCreateAnswer(
            IntPtr connection,
            IntPtr observer,
            PeerConnectionInterface.RtcOfferAnswerOptions options
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void webrtcPeerConnectionInterfaceCreateOffer(
            IntPtr connection,
            IntPtr observer,
            PeerConnectionInterface.RtcOfferAnswerOptions options
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void webrtcPeerConnectionInterfaceClose(
            IntPtr connection
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr webrtcPeerConnectionInterfaceGetSenders(
            IntPtr connection
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void webrtcPeerConnectionInterfaceSetAudioRecording(
            IntPtr connection,
            [MarshalAs(UnmanagedType.I1)] bool recording
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void webrtcPeerConnectionInterfaceSetLocalDescription(
            IntPtr connection,
            IntPtr observer,
            IntPtr desc
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void webrtcPeerConnectionInterfaceSetRemoteDescription(
            IntPtr connection,
            IntPtr observer,
            IntPtr desc
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void webrtcRtpSenderInterfacesMove(
            IntPtr cplusplus,
            IntPtr c
        );

        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr webrtcRtpSenderInterfacesSize(
            IntPtr interfaces
        );

        public static bool AddIceCandidate(
            this IPeerConnectionInterface connection,
            IIceCandidateInterface candidate)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (candidate == null)
            {
                throw new ArgumentNullException(nameof(candidate));
            }

            var result = webrtcPeerConnectionInterfaceAddIceCandidate(
                connection.Ptr, candidate.Ptr);

            GC.KeepAlive(connection);
            GC.KeepAlive(candidate);

            return result;
        }

        public static RtcErrorOr<DisposableRtpSenderInterface> AddTrack(
            this IPeerConnectionInterface connection,
            IMediaStreamTrackInterface track,
            string[] streamIds)
        {
            RtcError error;

            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (track == null)
            {
                throw new ArgumentNullException(nameof(track));
            }

            if (streamIds == null)
            {
                throw new ArgumentNullException(nameof(streamIds));
            }

            var size = new UIntPtr((ulong)streamIds.LongLength);
            var result = webrtcPeerConnectionInterfaceAddTrack(
                connection.Ptr, track.Ptr, streamIds, size);

            GC.KeepAlive(connection);
            GC.KeepAlive(track);

            try
            {
                error = new RtcError(result.Error);
            }
            finally
            {
                Interop.RtcError.Delete(result.Error);
            }

            var value = error.Type == RtcErrorType.None ?
                new DisposableRtpSenderInterface(result.Value) : null;

            return new RtcErrorOr<DisposableRtpSenderInterface>(error, value);
        }

        public static void Close(this IPeerConnectionInterface connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            webrtcPeerConnectionInterfaceClose(connection.Ptr);
            GC.KeepAlive(connection);
        }

        public static void CreateAnswer(
            this IPeerConnectionInterface connection,
            ICreateSessionDescriptionObserver observer,
            PeerConnectionInterface.RtcOfferAnswerOptions options)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            webrtcPeerConnectionInterfaceCreateAnswer(
                connection.Ptr, observer.Ptr, options);

            GC.KeepAlive(connection);
            GC.KeepAlive(observer);
        }

        public static void CreateOffer(
            this IPeerConnectionInterface connection,
            ICreateSessionDescriptionObserver observer,
            PeerConnectionInterface.RtcOfferAnswerOptions options)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            webrtcPeerConnectionInterfaceCreateOffer(
                connection.Ptr, observer.Ptr, options);

            GC.KeepAlive(connection);
            GC.KeepAlive(observer);
        }

        public static DisposableRtpSenderInterface[] GetSenders(
            this IPeerConnectionInterface connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var cplusplus =
                webrtcPeerConnectionInterfaceGetSenders(connection.Ptr);
            GC.KeepAlive(connection);
            var left = (long)webrtcRtpSenderInterfacesSize(cplusplus);
            var interfaces = new DisposableRtpSenderInterface[left];
            var c = new IntPtr[left];

            var handle = GCHandle.Alloc(c, GCHandleType.Pinned);
            try
            {
                var addr = handle.AddrOfPinnedObject();
                webrtcRtpSenderInterfacesMove(cplusplus, addr);
            }
            finally
            {
                handle.Free();
            }

            while (left > 0)
            {
                left--;
                interfaces[left] = new DisposableRtpSenderInterface(c[left]);
            }

            return interfaces;
        }

        public static void SetAudioRecording(
            this IPeerConnectionInterface connection,
            bool recording)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            webrtcPeerConnectionInterfaceSetAudioRecording(
                connection.Ptr,
                recording
            );

            GC.KeepAlive(connection);
        }

        public static void SetLocalDescription(
            this IPeerConnectionInterface connection,
            ISetSessionDescriptionObserver observer,
            IDisposableSessionDescriptionInterface desc)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            if (desc == null)
            {
                throw new ArgumentNullException(nameof(desc));
            }

            webrtcPeerConnectionInterfaceSetLocalDescription(
                connection.Ptr, observer.Ptr, desc.Ptr);

            GC.KeepAlive(connection);
            GC.KeepAlive(observer);
            desc.ReleasePtr();
        }

        public static void SetRemoteDescription(
            this IPeerConnectionInterface connection,
            ISetSessionDescriptionObserver observer,
            IDisposableSessionDescriptionInterface desc)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            if (desc == null)
            {
                throw new ArgumentNullException(nameof(desc));
            }

            webrtcPeerConnectionInterfaceSetRemoteDescription(
                connection.Ptr, observer.Ptr, desc.Ptr);

            GC.KeepAlive(connection);
            GC.KeepAlive(observer);
            desc.ReleasePtr();
        }
    }
}

namespace Pixiv.Webrtc.Interop
{
    public static class PeerConnectionFactoryInterface
    {
        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "webrtcPeerConnectionFactoryInterfaceRelease")]
        public static extern void Release(IntPtr factory);
    }

    public static class PeerConnectionInterface
    {
        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "webrtcPeerConnectionInterfaceRelease")]
        public static extern void Release(IntPtr ptr);
    }

    public static class PeerConnectionObserver
    {
        [DllImport(Dll.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "webrtcDeletePeerConnectionObserver")]
        public static extern void Delete(IntPtr observer);
    }
}

using System.Runtime.CompilerServices;
using JasperFx;
using JasperFx.Core;
using JasperFx.Core.TypeScanning;

[assembly:IgnoreAssembly]
[assembly:JasperFxTool]
[assembly:JasperFxAssembly]

[assembly: InternalsVisibleTo("Marten.Testing")]
[assembly: InternalsVisibleTo("Marten.CommandLine")]
[assembly: InternalsVisibleTo("Marten.PLv8")]
[assembly: InternalsVisibleTo("Marten.PLv8.Testing")]
[assembly: InternalsVisibleTo("Marten.Schema.Testing")]
[assembly: InternalsVisibleTo("DaemonTests")]
[assembly: InternalsVisibleTo("ConfigurationTests")]
[assembly: InternalsVisibleTo("CoreTests")]
[assembly: InternalsVisibleTo("MultiTenancyTests")]
[assembly: InternalsVisibleTo("LinqTests")]
[assembly: InternalsVisibleTo("DocumentDbTests")]
[assembly: InternalsVisibleTo("EventSourcingTests")]
[assembly: InternalsVisibleTo("Examples")]
[assembly: InternalsVisibleTo("PatchingTests")]
[assembly: InternalsVisibleTo("StressTests")]
[assembly: InternalsVisibleTo("ContainerScopedProjectionTests")]

using ApprovalTests.Namers;
using ApprovalTests.Reporters;

[assembly: UseReporter(typeof(ClipboardReporter), typeof(DiffReporter))]
[assembly: UseApprovalSubdirectory("Approved")]

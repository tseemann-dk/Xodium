using Xodium.Productivity.Scheduling;
using MSGraph = Microsoft.Graph;

namespace Xodium.Productivity.Microsoft365.Extensions
{
    public static class EnumExtensions
    {
        public static Availability ToAvailability(this MSGraph.FreeBusyStatus self)
        {
            switch (self)
            {
                case MSGraph.FreeBusyStatus.Free:
                    return Availability.Free;
                case MSGraph.FreeBusyStatus.Tentative:
                    return Availability.Tentative;
                case MSGraph.FreeBusyStatus.Busy:
                    return Availability.Busy;
                case MSGraph.FreeBusyStatus.Oof:
                    return Availability.Unavailable;
                case MSGraph.FreeBusyStatus.WorkingElsewhere:
                    return Availability.Elsewhere;
                case MSGraph.FreeBusyStatus.Unknown:
                default:
                    return Availability.Undefined;
            }
        }

        public static Importance ToImportance(this MSGraph.Importance self)
        {
            switch (self)
            {
                case MSGraph.Importance.Low:
                    return Importance.Low;
                case MSGraph.Importance.Normal:
                    return Importance.Normal;
                case MSGraph.Importance.High:
                    return Importance.High;
                default:
                    return Importance.Normal;
            }
        }

        public static Sensitivity ToSensitivity(this MSGraph.Sensitivity self)
        {
            switch (self)
            {
                case MSGraph.Sensitivity.Normal:
                    return Sensitivity.Public;
                case MSGraph.Sensitivity.Personal:
                case MSGraph.Sensitivity.Private:
                    return Sensitivity.Private;
                case MSGraph.Sensitivity.Confidential:
                    return Sensitivity.Confidential;
                default:
                    return Sensitivity.Public;
            }
        }
    }
}

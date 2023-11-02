using Xodium.Productivity.Scheduling;
using MSGraphModels = Microsoft.Graph.Models;

namespace Xodium.Productivity.Microsoft365.Extensions
{
    public static class EnumExtensions
    {
        public static Availability ToAvailability(this MSGraphModels.FreeBusyStatus self)
        {
            switch (self)
            {
                case MSGraphModels.FreeBusyStatus.Free:
                    return Availability.Free;
                case MSGraphModels.FreeBusyStatus.Tentative:
                    return Availability.Tentative;
                case MSGraphModels.FreeBusyStatus.Busy:
                    return Availability.Busy;
                case MSGraphModels.FreeBusyStatus.Oof:
                    return Availability.Unavailable;
                case MSGraphModels.FreeBusyStatus.WorkingElsewhere:
                    return Availability.Elsewhere;
                case MSGraphModels.FreeBusyStatus.Unknown:
                default:
                    return Availability.Undefined;
            }
        }

        public static Importance ToImportance(this MSGraphModels.Importance self)
        {
            switch (self)
            {
                case MSGraphModels.Importance.Low:
                    return Importance.Low;
                case MSGraphModels.Importance.Normal:
                    return Importance.Normal;
                case MSGraphModels.Importance.High:
                    return Importance.High;
                default:
                    return Importance.Normal;
            }
        }

        public static Sensitivity ToSensitivity(this MSGraphModels.Sensitivity self)
        {
            switch (self)
            {
                case MSGraphModels.Sensitivity.Normal:
                    return Sensitivity.Public;
                case MSGraphModels.Sensitivity.Personal:
                case MSGraphModels.Sensitivity.Private:
                    return Sensitivity.Private;
                case MSGraphModels.Sensitivity.Confidential:
                    return Sensitivity.Confidential;
                default:
                    return Sensitivity.Public;
            }
        }
    }
}

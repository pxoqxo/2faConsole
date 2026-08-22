using Pxoqxo.Quick;

namespace _2faConsole
{
    public static class OtpFile
    {
        public static OtpFileDm? Read()
        {
            string? json = QuickFile.Read(PresetPaths.Otps);
            if (json == null)
            {
                return null;
            }

            return QuickJson.FromJson<OtpFileDm>(json);
        }
        public static bool Write(OtpFileDm otpFileDm)
        {
            string? json = QuickJson.ToJson(otpFileDm);
            if (json == null)
            {
                return false;
            }

            return QuickFile.Write(PresetPaths.Otps, json);
        }
    }
}

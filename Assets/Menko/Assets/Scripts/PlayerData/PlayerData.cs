using System.Collections.Generic;
using System.Linq;
using Menko.Enums;

namespace Menko.PlayerData
{
    [System.Serializable]
    public class PlayerData
    {
        public List<MenkoSetting> MenkoSettings;
        public List<MenkoAchievement> MenkoAchievements;

        public static PlayerData Init(List<MenkoData> MenkoDatas)
        {
            PlayerData newPlayerData = new()
            {
                MenkoAchievements = InitMenkoAchievements(MenkoDatas),
                MenkoSettings = InitMenkoSettings(MenkoDatas)
            };
            return newPlayerData;
        }

        private static List<MenkoAchievement> InitMenkoAchievements(List<MenkoData> MenkoDatas)
        {
            IEnumerable<MenkoAchievement> newMenkoAchievements = MenkoDatas.Select(data =>
            {
                MenkoAchievement newData = new()
                {
                    id = data.GetId(),
                    isOpen = data.GetRank() == Rank.Default
                };
                return newData;
            });
            return newMenkoAchievements.ToList();
        }

        private static List<MenkoSetting> InitMenkoSettings(List<MenkoData> MenkoDatas)
        {
            IEnumerable<MenkoSetting> newMenkoSetting = MenkoDatas.Select(data =>
            {
                if (data.GetRank() != Rank.Default) return null;

                int id = data.GetId();
                MenkoSetting newData = new()
                {
                    id = id,
                    index = id
                };
                return newData;
            });
            return newMenkoSetting.Where(data => data != null).ToList();
        }

        public bool CheckAllAchievementOpen()
        {
            return !MenkoAchievements.Exists(a => a.isOpen == false);
        }

        public void UpdateMenkoSetting(int index, int updateId)
        {
            MenkoSetting MenkoSetting = MenkoSettings.Find(match: a => a.index == index);
            MenkoSetting.UpdateMenkoId(updateId);
        }

        public void UpdateMenkoAchievement(int id, bool updateBool)
        {
            MenkoAchievement MenkoAchievement = MenkoAchievements.Find(match: a => a.id == id);
            MenkoAchievement.UpdateIsOpen(updateBool);
        }

        public MenkoAchievement GetMenkoAchievementById(int id)
        {
            MenkoAchievement MenkoAchievement = MenkoAchievements.Find(match: a => a.id == id);
            return MenkoAchievement;
        }

        public MenkoSetting GetMenkoSettingById(int id)
        {
            MenkoSetting MenkoSetting = MenkoSettings.Find(match: a => a.id == id);
            return MenkoSetting;
        }

        public MenkoSetting GetMenkoSettingByIndex(int index)
        {
            MenkoSetting MenkoSetting = MenkoSettings.Find(match: a => a.index == index);
            return MenkoSetting;
        }
    }

    [System.Serializable]
    public class MenkoAchievement
    {
        public int id; // ¯•ÊMenkoID
        public bool isOpen; // ‰ğœ‚µ‚Ä‚¢‚é‚©”Û‚©

        public void UpdateIsOpen(bool isOpen)
        {
            this.isOpen = isOpen;
        }
    }

    [System.Serializable]
    public class MenkoSetting
    {
        public int index; // 1: ƒƒCƒ“ | 2, 3: ƒTƒu
        public int id; // ¯•ÊMenkoID

        public void UpdateMenkoId(int id)
        {
            this.id = id;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Menko.Enums;

namespace Menko.PlayerData
{
    [System.Serializable]
    public class PlayerData
    {
        public int SetMenkoId;
        public List<MenkoAchievement> MenkoAchievements;

        public static PlayerData Init(List<MenkoData> MenkoDatas)
        {
            PlayerData newPlayerData = new()
            {
                MenkoAchievements = InitMenkoAchievements(MenkoDatas),
                SetMenkoId = MenkoDatas[0].GetId()
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

        public bool CheckAllAchievementOpen()
        {
            return !MenkoAchievements.Exists(a => a.isOpen == false);
        }

        public void UpdateMenkoSetting(int updateId)
        {
            SetMenkoId = updateId;
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
}

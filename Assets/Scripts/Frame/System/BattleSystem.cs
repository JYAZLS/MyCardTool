  using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace CardGameApp
{
    public interface IBattleSystem : ISystem
    {
        public ICharacter Hero { get; set;}
        public void DragCharacter(ICharacter hero,Vector3 vector3);
        public void PlaceCharacter(ICharacter hero);
        public void ReleaseCharacter(ICharacter hero);
        public void DeleteCharacter(ICharacter _object);
        public int TeamTotalNum { get;}
        public BindableProperty<int> TeamNum {get;}
        public BindableProperty<int> RoundNum{get;}
        public ICharacter GetCharacter(Vector3 vec3);
    }
    public class BattleSystem : AbstractSystem, IBattleSystem
    {
        private IGameModel mModel;
        public int TeamTotalNum { 
            get { return mModel.playerInfo.PlayerTeamNumber; } 
            set { mModel.playerInfo.PlayerTeamNumber = value; }
        }

        public BindableProperty<int> TeamNum {get;set;} = new BindableProperty<int>(0);
        public BindableProperty<int> RoundNum {get;set;}= new BindableProperty<int>(0);
        public List<List<ICharacter>> mBattleUnit {get;set;} = new List<List<ICharacter>>();
        public ICharacter Hero { get; set; }
        
        protected override void OnInit()
        {
            mModel = this.GetModel<IGameModel>();          
            Hero = null;
        }

        /// <summary>
        /// 拖拽人物
        /// </summary>
        /// <param name="vector3"></param>
        public void DragCharacter(ICharacter hero,Vector3 vector3)
        {
            hero.mGameObject.transform.position = new Vector3(vector3.x + 0.24f, vector3.y + 0.24f, 0);
        }
        /// <summary>
        /// 放置人物
        /// </summary>
        public void PlaceCharacter(ICharacter hero)
        {
            if (mBattleUnit.Count < TeamNum + 1)
            {
                mBattleUnit.Add(new List<ICharacter>());
                mBattleUnit[TeamNum].Add(hero);
            }
            else
            {
                mBattleUnit[TeamNum].Add(hero);
            }
            hero.SetTeam(TeamNum);
        }
        public void ReleaseCharacter(ICharacter hero)
        {
            if (hero != null)
            {
                Dictionary<string, CharacterPool> CharacterPlayerPool = ResManager.Intance.CharacterPlayerPool;
                CharacterPlayerPool[hero.CharacterAttr.baseName].Release(hero.mGameObject);
            }    
        }

        public ICharacter GetCharacter(Vector3 vec3)
        {
            vec3.x += 0.24f;
            vec3.y += 0.24f;
            foreach(var team in mBattleUnit)
            {
                foreach(var it in team)
                {
                    if(it.mGameObject.transform.position == vec3)
                    {
                        return it;
                    }
                }
            }
            return null;
        }

        public void DeleteCharacter(ICharacter _object)
        {
            foreach(var team in mBattleUnit)
            {
                foreach(var it in team)
                {
                    if(it.mGameObject.transform == _object.mGameObject.transform)
                    {
                        Dictionary<string, CharacterPool> CharacterPlayerPool = ResManager.Intance.CharacterPlayerPool;
                        CharacterPlayerPool[_object.CharacterAttr.baseName].Release(_object.mGameObject);
                        team.Remove(it);
                        return;
                    }
                }
            }
        }
    }
}

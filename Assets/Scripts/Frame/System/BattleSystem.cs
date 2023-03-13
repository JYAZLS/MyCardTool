  using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace CardGameApp
{
    public interface IBattleSystem : ISystem
    {
        public ICharacter ChooseCharacter { get; }
        public void SetChooseCharacter(ICharacter _object);
        public void ClearChooseCharacter();
        public void DragCharacter(Vector3 vector3);
        public void PlaceCharacter();
        public void ReleaseCharacter();
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
        public ICharacter ChooseCharacter { get; set; }
        
        protected override void OnInit()
        {
            mModel = this.GetModel<IGameModel>();          
            ChooseCharacter = null;
        }
        /// <summary>
        /// 设置选择的人物
        /// </summary>
        /// <param name="_object"></param>
        public void SetChooseCharacter(ICharacter _object)
        {
            ChooseCharacter = _object;
        }
        /// <summary>
        /// 清空选择
        /// </summary>
        public void ClearChooseCharacter()
        {
            ChooseCharacter = null;
        }
        /// <summary>
        /// 拖拽人物
        /// </summary>
        /// <param name="vector3"></param>
        public void DragCharacter(Vector3 vector3)
        {
            ChooseCharacter.mGameObject.transform.position = new Vector3(vector3.x + 0.24f, vector3.y + 0.24f, 0);
        }
        /// <summary>
        /// 放置人物
        /// </summary>
        public void PlaceCharacter()
        {
            if (mBattleUnit.Count < TeamNum + 1)
            {
                mBattleUnit.Add(new List<ICharacter>());
                mBattleUnit[TeamNum].Add(ChooseCharacter);
            }
            else
            {
                mBattleUnit[TeamNum].Add(ChooseCharacter);
            }
            ChooseCharacter.SetTeam(TeamNum);
            ChooseCharacter = null;
        }
        public void ReleaseCharacter()
        {
            if (ChooseCharacter != null)
            {
                Dictionary<string, CharacterPool> CharacterPlayerPool = ResManager.Intance.CharacterPlayerPool;
                CharacterPlayerPool[ChooseCharacter.CharacterAttr.baseName].Release(ChooseCharacter.mGameObject);
                ChooseCharacter = null;
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

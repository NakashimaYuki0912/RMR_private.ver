using System;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using CustomMapUtility;
using abcdcode_LOGLIKE_MOD;


namespace RogueLike_Mod_Reborn
{
    public class EnemyTeamStageManager_AutoStartMysteryStage : EnemyTeamStageManager
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            Singleton<MysteryManager>.Instance.StartMystery(LogLikeMod.curstageid);
        }
    }

    public class CryingChildrenLogKys : MapManager
    {
        public void PlayAreaLaserSound()
        {
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip(this._areaLaserSound, false, 1f, null);
        }

        public void ChangeMap()
        {
            SingletonBehavior<CreatureDlgManagerUI>.Instance.Init(SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject == this);
            if (this._currentPhase != this._stageManager.currentPhase)
            {
                this._currentPhase = this._stageManager.currentPhase;
                this.ChangeMap(this._currentPhase);
                if (this._currentPhase == EnemyTeamStageManager_TheCrying.Phase.OneUnitPhase)
                {
                    SingletonBehavior<SoundEffectManager>.Instance.PlayClip(this._makeOneSound, false, 1f, null);
                }
                else
                {
                    SingletonBehavior<SoundEffectManager>.Instance.PlayClip(this._seperateSound, false, 1f, null);
                }
                if (!this._bgChanged)
                {
                    this._bgChanged = true;
                    this.mapBgm[0] = this._nextBgm;
                    SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(this.mapBgm);
                    SingletonBehavior<BattleSoundManager>.Instance.ChangeEnemyTheme(0);
                    if (this._loopWhisperSound != null)
                    {
                        this._loopWhisperSound.ManualDestroy();
                        this._loopWhisperSound = null;
                    }
                    this._loopWhisperSound = SingletonBehavior<SoundEffectManager>.Instance.PlayClip(this._whisperBgm, true, 1f, base.transform);
                }
            }
            this.EnableAngelParticle();
            if (this._loopWhisperSound != null)
            {
                if (this._stageManager.Stack >= 5)
                {
                    this._loopWhisperSound.SetVolume(0.8f * GlobalGameManager.Instance.CurrentOption.GetVolumeEffect());
                    return;
                }
                if (this._stageManager.Stack >= 3)
                {
                    this._loopWhisperSound.SetVolume(0.4f * GlobalGameManager.Instance.CurrentOption.GetVolumeEffect());
                    return;
                }
                this._loopWhisperSound.SetVolume(0.2f * GlobalGameManager.Instance.CurrentOption.GetVolumeEffect());
            }
        }

        public override void Awake()
        {
            base.Awake();
            CryingChildMapManager mapManager = SingletonBehavior<BattleSceneRoot>.Instance.transform.Find("InvitationMap_CryingChild").GetComponent<MapManager>() as CryingChildMapManager;

            this.isActivated = mapManager.isActivated;
            this.isEnabled = mapManager.isEnabled;
            this.mapSize = mapManager.mapSize;
            this.sephirahType = mapManager.sephirahType;
            this.borderFrame = mapManager.borderFrame;
            this.backgroundRoot = mapManager.backgroundRoot;
            this.sephirahColor = mapManager.sephirahColor;
            this.scratchPrefabs = mapManager.scratchPrefabs;
            this.wallCratersPrefabs = mapManager.wallCratersPrefabs;
            this._roots = mapManager._roots;
            this._obstacleRoot = mapManager._obstacleRoot;
            this.mapBgm = mapManager.mapBgm;
            this._obstacles = mapManager._obstacles;

            this._burnPhaseSound = Instantiate<AudioClip>(mapManager._burnPhaseSound, this.transform.parent);
            this._angelParticles = new List<GameObject>();
            foreach (var particle in mapManager._angelParticles)
                this._angelParticles.Add(Instantiate<GameObject>(particle, particle.transform.parent));
            this._sparkParticle = Instantiate(mapManager._sparkParticle, mapManager._sparkParticle.transform.parent);
            this._ashParticle = Instantiate(mapManager._ashParticle, mapManager._ashParticle.transform.parent);
            this._areaLaserSound = Instantiate<AudioClip>(mapManager._areaLaserSound, this.transform.parent);
            this._ashPhaseSound = Instantiate<AudioClip>(mapManager._ashPhaseSound, this.transform.parent);
            this._mapChangeFilter = Instantiate(mapManager._mapChangeFilter, mapManager._mapChangeFilter.transform.parent);
            this._burnSpriteRenderers = new List<SpriteRenderer>();
            foreach (var render in mapManager._burnSpriteRenderers)
                this._burnSpriteRenderers.Add(Instantiate(render, render.transform.parent));
            this._dlgColor = mapManager._dlgColor;
            this._makeOneSound = Instantiate<AudioClip>(mapManager._makeOneSound, this.transform.parent);
            this._seperateSound = Instantiate<AudioClip>(mapManager._seperateSound, this.transform.parent);
            this._whisperBgm = Instantiate<AudioClip>(mapManager._whisperBgm, this.transform.parent);
            this._nextBgm = Instantiate<AudioClip>(mapManager._nextBgm, this.transform.parent);
        }

        public override void InitializeMap()
        {
            base.InitializeMap();
            EnemyTeamStageManager enemyStageManager = Singleton<StageController>.Instance.EnemyStageManager;
            if (enemyStageManager != null && enemyStageManager is EnemyTeamStageManager_TheCryingLog)
            {
                this._stageManager = (enemyStageManager as EnemyTeamStageManager_TheCryingLog);
                this._currentPhase = this._stageManager.currentPhase;
            }
            foreach (SpriteRenderer spriteRenderer in this._burnSpriteRenderers)
            {
                spriteRenderer.enabled = false;
            }
            this._dlgIdList.Add("ChildCovering_nomal_1");
            this._dlgIdList.Add("ChildCovering_nomal_2");
            this._dlgIdList.Add("ChildCovering_nomal_3");
            this._dlgIdList.Add("ChildCovering_nomal_4");
            this._dlgIdList.Add("ChildCovering_nomal_5");
            this._dlgIdList.Add("ChildCovering_nomal_6");
            this._dlgIdList.Add("ChildCovering_nomal_7");
            this._dlgIdList.Add("ChildCovering_nomal_8");
            this._dlgIdList.Add("TheCryingChildren_nomal_1");
            this._dlgIdList.Add("TheCryingChildren_nomal_2");
            this._dlgIdList.Add("TheCryingChildren_nomal_3");
            this._dlgIdList.Add("TheCryingChildren_nomal_4");
            this._dlgIdList.Add("TheCryingChildren_nomal_5");
            this._dlgIdList.Add("TheCryingChildren_nomal_6");
            this._dlgIdList.Add("TheCryingChildren_nomal_7");
            this._dlgIdList.Add("TheCryingChildren_nomal_8");
            this._dlgIdList.Add("TheCryingChildren_nomal_9");
            this._dlgIdList.Add("TheCryingChildren_nomal_10");
            this._dlgIdList.Add("TheCryingChildren_nomal_11");
            this._dlgIdList.Add("TheCryingChildren_nomal_12");
            this.CreateDialog();
            SingletonBehavior<CreatureDlgManagerUI>.Instance.Init(SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject == this);
            this._bgChanged = false;
        }

        public override void EnableMap(bool b)
        {
            base.EnableMap(b);
            if (this._loopWhisperSound != null)
            {
                this._loopWhisperSound.gameObject.SetActive(b);
            }
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.ChangeMap();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            AudioSource currentPlayingTheme = SingletonBehavior<BattleSoundManager>.Instance.CurrentPlayingTheme;
            if (currentPlayingTheme != null && currentPlayingTheme.clip == this._nextBgm && currentPlayingTheme.isPlaying && currentPlayingTheme.timeSamples > 9621773)
            {
                currentPlayingTheme.timeSamples = 3848791;
            }
        }

        public void Update()
        {
            if (this._dlgEffect != null && this._dlgEffect.gameObject != null)
            {
                if (this._dlgEffect.DisplayDone)
                {
                    this._dlgIdx++;
                    this.CreateDialog();
                    return;
                }
            }
            else
            {
                this.CreateDialog();
            }
        }

        public void CreateDialog()
        {
            if (this._currentPhase == EnemyTeamStageManager_TheCrying.Phase.OneUnitPhase)
            {
                this._dlgIdx = this._dlgIdx % 12 + 8;
                this._dlgColor = Color.black;
            }
            else
            {
                this._dlgIdx %= 8;
                this._dlgColor = new Color(0.8f, 0f, 0f);
            }
            if (this._dlgIdList.Count <= 0 || this._dlgIdx >= this._dlgIdList.Count)
            {
                return;
            }
            string text = TextDataModel.GetText(this._dlgIdList[this._dlgIdx], Array.Empty<object>());
            if (this._dlgEffect != null && this._dlgEffect.gameObject != null)
            {
                this._dlgEffect.FadeOut();
            }
            this._dlgEffect = SingletonBehavior<CreatureDlgManagerUI>.Instance.SetDlg(text, this._dlgColor, null);
        }

        public void EnableAngelParticle()
        {
            if (this._currentPhase == EnemyTeamStageManager_TheCrying.Phase.OneUnitPhase)
            {
                foreach (GameObject gameObject in this._angelParticles)
                {
                    gameObject.SetActive(false);
                }
                this._ashParticle.SetActive(false);
                this._sparkParticle.SetActive(true);
                return;
            }
            if (this._currentPhase == EnemyTeamStageManager_TheCrying.Phase.FiveUnitPhase)
            {
                this._ashParticle.SetActive(true);
                this._sparkParticle.SetActive(false);
                foreach (GameObject gameObject2 in this._angelParticles)
                {
                    gameObject2.SetActive(false);
                }
                List<GameObject> list = new List<GameObject>(this._angelParticles);
                for (int i = 0; i < list.Count - 1; i++)
                {
                    int index = UnityEngine.Random.Range(i, list.Count);
                    GameObject value = list[i];
                    list[i] = list[index];
                    list[index] = value;
                }
                int j = this._stageManager.Stack * 2;
                while (j > 0)
                {
                    if (list.Count <= 0)
                    {
                        return;
                    }
                    j--;
                    GameObject gameObject3 = list[UnityEngine.Random.Range(0, list.Count)];
                    gameObject3.SetActive(true);
                    list.Remove(gameObject3);
                }
            }
            else
            {
                this._ashParticle.SetActive(true);
                this._sparkParticle.SetActive(false);
                foreach (GameObject gameObject4 in this._angelParticles)
                {
                    gameObject4.SetActive(false);
                }
            }
        }

        public void ChangeMap(EnemyTeamStageManager_TheCrying.Phase phase)
        {
            this._mapChangeFilter.StartMapChangingEffect(Direction.LEFT, true);
            bool enabled = phase == EnemyTeamStageManager_TheCrying.Phase.OneUnitPhase;
            foreach (SpriteRenderer spriteRenderer in this._burnSpriteRenderers)
            {
                spriteRenderer.enabled = enabled;
            }
            if (phase == EnemyTeamStageManager_TheCrying.Phase.OneUnitPhase)
            {
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip(this._burnPhaseSound, false, 1f, null);
                return;
            }
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip(this._ashPhaseSound, false, 1f, null);
        }

        public void OnDestroy()
        {
            if (this._loopWhisperSound != null)
            {
                this._loopWhisperSound.ManualDestroy();
                this._loopWhisperSound = null;
            }
        }

        public void OnDisable()
        {
            if (this._loopWhisperSound != null)
            {
                this._loopWhisperSound.ManualDestroy();
                this._loopWhisperSound = null;
            }
        }

        [SerializeField]
        public AudioClip _burnPhaseSound;

        [SerializeField]
        public AudioClip _ashPhaseSound;

        [SerializeField]
        public AudioClip _nextBgm;

        [SerializeField]
        public AudioClip _whisperBgm;

        [SerializeField]
        public AudioClip _areaLaserSound;

        [SerializeField]
        public AudioClip _seperateSound;

        [SerializeField]
        public AudioClip _makeOneSound;

        [SerializeField]
        public List<SpriteRenderer> _burnSpriteRenderers;

        [SerializeField]
        public Color _dlgColor;

        [SerializeField]
        public MapChangeFilter _mapChangeFilter;

        [SerializeField]
        public List<GameObject> _angelParticles;

        [SerializeField]
        public GameObject _ashParticle;

        [SerializeField]
        public GameObject _sparkParticle;

        public EnemyTeamStageManager_TheCryingLog _stageManager;

        public EnemyTeamStageManager_TheCrying.Phase _currentPhase;

        public List<string> _dlgIdList = new List<string>();

        public CreatureDlgEffectUI _dlgEffect;

        public int _dlgIdx;

        public bool _bgChanged;

        public SoundEffectPlayer _loopWhisperSound;
    }

    public class SparklingMirrorMapManager : CustomMapManager
    {

    }

}

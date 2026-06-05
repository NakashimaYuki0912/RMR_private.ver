public void LoadTuto(string contentname)
{
	UIManualContentPanel.instance.SetData();
	TimeManager.PauseTime();
	UIManualContentPanel.instance.OpenOnlyContent();
	UIManualContentId currentid = this.FindLogTuto(contentname).Tutopage.currentid;
	UIManualContentPanel.instance.ShowContents(currentid);
	UIManualPanel.instance.revealAnim.enabled = true;
	UIManualPanel.instance.revealAnim.SetTrigger("Reveal");
	bool flag = this.IsSeeTutoDic.ContainsKey(contentname);
	if (flag)
	{
		this.IsSeeTutoDic[contentname] = true;
	}
	Singleton<LogueSaveManager>.Instance.SaveData(this.CreateSaveData(), "Tutorial");
}
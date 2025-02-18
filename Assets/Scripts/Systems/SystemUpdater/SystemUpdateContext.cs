using MergeCase.General.Interfaces;
using MergeCase.Systems.Gameplay;

namespace MergeCase.Systems.Updater
{
	public class SystemUpdateContext<T> where T : SystemBase
	{
#if ODIN_INSPECTOR
		[Sirenix.OdinInspector.ShowInInspector]
#endif
		public IDataCollection DataCollection { get; private set; }

#if ODIN_INSPECTOR
		[Sirenix.OdinInspector.ShowInInspector]
#endif
		public SystemUpdater<T> SystemUpdater { get; private set; }

#if ODIN_INSPECTOR
		[Sirenix.OdinInspector.ShowInInspector]
#endif
		public GameStateData GameState { get; private set; }

		public SystemUpdateContext(IDataCollection dataCollection, SystemUpdater<T> systemUpdater, GameStateData gameStateData)
		{
			DataCollection = dataCollection;
			SystemUpdater = systemUpdater;
			GameState = gameStateData;
		}
	}
}
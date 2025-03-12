Load sequence allows you to organize the loading stage and transition between Unity project states.

Dependencies: UniTask, Unity

Example:

```csharp
loadSequence.
  Sequential<InitializeNetConnectionServiceStep>().
  Parallel<DefaultAddressablesPreloadSequenceStep>().
  Sequential<StatisticInitializeStep>().
  Sequential<ServiceInitializeSequenceStep>().
  Sequential<AnalyticsInitializeSequenceStep>().
  Sequential<SettingsInitializeSequenceStep>()..
  Sequential<VfxInitializeSequenceStep>().
  Sequential<LocalizationInitializeSequenceStep>().
  Sequential<LoadGameSceneSequenceStep>().
  Sequential<ScaleServiceInitializeStep>().
  Sequential<CreateGameSceneCameraStep>().
  Sequential<UiServicePrepareSequenceStep>().
  Sequential<InputServiceInitializeStep>().
  Sequential<GameplaySceneSetupSequenceStep>().
  Sequential<EnableAndAdjustUiByBannerPlaceSequenceStep>().
  Sequential<DebugServiceInitializeSequenceStep>(condition: isDebug).
  Sequential<StartGameplaySceneStep>().
  Sequential<UnloadPreLaunchSceneSequenceStep>().
  End();
```

There is also the possibility of adding an honest loading bar that receives progress information from the steps.

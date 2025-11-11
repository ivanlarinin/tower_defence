Game Link: https://veltrynox.itch.io/ironclash-alpha

# 🏰 Tower Defence (Unity Project)

A fully-featured **Tower Defence game framework** built in **Unity**, featuring wave-based enemy spawning, tower building, upgrades, map progression, and persistent player data.
All core game logic is implemented in C# across modular scripts organized under the `TowerDefence` namespace.

---

## 🎮 Features

* **Tower System**

  * Configurable `TowerAssets` for each tower type (cost, sprite, upgrade chain, turret setup).
  * Dynamic **buy/build UI** for constructing towers on available `BuildSite`s.
  * Real-time targeting and projectile prediction.

* **Enemy System**

  * Data-driven enemies using `EnemyAsset` ScriptableObjects.
  * Modular AI controller (`AIController`, `TDPatrolController`) for path-based movement.
  * Armor types, buffs, and on-death gold rewards.

* **Wave Management**

  * JSON-configurable wave definitions (`WaveConfig` → `EnemyWavesManager`).
  * Automatic wave scheduling and manual early start support.
  * Events for UI binding (wave number display, wave completion).

* **Progression & Save System**

  * Persistent progression via `Saver<T>` and `FileHandler`.
  * `MapCompletion` tracks level scores and total progress.
  * Upgrade system (`UpgradeAsset`, `Upgrades`, `UpgradeShop`) using saved data.
  * Unlockable levels (`BranchLevel`, `MapLevel`).

* **UI & Game States**

  * Menus: Main Menu, Pause Menu, Level Results, Upgrade Shop.
  * Live UI updates for gold, lives, wave count (`UI_TextUpdate`).
  * Win/Lose detection with sound and star scoring (`LevelResult`, `LevelController`).

* **Audio System**

  * Global sound controller (`SoundPlayer`) using enum-based extension calls like:

    ```csharp
    Sound.Arrow.Play();
    ```
  * Auto BGM switching between gameplay and menu scenes.

* **Data-Driven Design**

  * Tower, Enemy, Upgrade, and Wave configurations stored as `ScriptableObject`s or JSON.
  * Extensible via Unity inspector without code changes.

---

## 🧩 Architecture Overview

```
Assets/
└── Scripts/
    ├── Core/
    │   ├── SingletonBase.cs
    │   ├── FileHandler.cs
    │   ├── Saver.cs
    │   └── Timer.cs
    ├── Player/
    │   ├── TDPlayer.cs
    │   ├── LevelController.cs
    │   ├── MapCompletion.cs
    │   └── UpgradeSystem/
    │       ├── UpgradeAsset.cs
    │       ├── Upgrades.cs
    │       ├── UpgradeShop.cs
    │       └── BuyUpgrade.cs
    ├── Towers/
    │   ├── Tower.cs
    │   ├── Turret.cs
    │   ├── Projectile.cs
    │   ├── TurretProperties.cs
    │   └── TowerAssets.cs
    ├── Enemies/
    │   ├── Enemy.cs
    │   ├── EnemyAsset.cs
    │   ├── EnemyPath.cs
    │   ├── EnemyWave.cs
    │   ├── EnemyWavesManager.cs
    │   └── TDPatrolController.cs
    ├── UI/
    │   ├── UI_TextUpdate.cs
    │   ├── MainMenu.cs
    │   ├── LevelResult.cs
    │   ├── PauseMenu.cs
    │   ├── BuyControl.cs
    │   └── TowerBuyControl.cs
    ├── Audio/
    │   ├── Sound.cs
    │   ├── SoundPlayer.cs
    │   └── SoundHook.cs
    └── Utility/
        ├── CircleArea.cs
        ├── FollowCamera.cs
        └── PointerClickHold.cs
```

---

## ⚙️ Setup Instructions

1. **Open in Unity**

   * Unity version: `2022.3 LTS` or later recommended.
   * Import the `Scripts` folder into your Unity project.

2. **Create Required Assets**

   * `EnemyAsset` – defines enemy stats and visuals.
   * `TowerAssets` – defines tower cost, sprites, and turret properties.
   * `TurretProperties` – projectile type, damage, rate of fire, etc.
   * `UpgradeAsset` – defines upgrade type and cost per level.
   * `Episode` – defines levels per episode (for campaign progression).

3. **Scene Setup**

   * Add `LevelSequenceController` to your persistent scene.
   * Add `SoundPlayer` prefab with assigned clips.
   * Include UI prefabs: MainMenu, HUD (with `UI_TextUpdate`), UpgradeShop, etc.

4. **Wave Data**

   * Create JSON in `Resources/<SceneName>_waves.json`:

     ```json
     {
       "waves": [
         {
           "prepareTime": 5,
           "groups": [
             {
               "pathIndex": 0,
               "squads": [
                 { "asset": "Goblin", "count": 5 }
               ]
             }
           ]
         }
       ]
     }
     ```

5. **Play**

   * Start from `LevelMap` or `MainMenu` scene.
   * Build towers, defeat waves, earn stars, and unlock upgrades.

---

## 🧠 Extending the Game

* Add new **enemy types** by creating new `EnemyAsset` files.
* Define new **towers** or **upgrades** through ScriptableObjects.
* Add **new levels** by expanding `Episode` assets and map links.
* Implement special abilities or tower effects by subclassing:

  * `ProjectileBase`
  * `Turret`
  * `Enemy`

---

## 💾 Save Data Locations

| File                                                  | Purpose                   |
| ----------------------------------------------------- | ------------------------- |
| `map_completion.json`                                 | Level progress and scores |
| `upgrades.dat`                                        | Upgrade levels per asset  |
| (auto-created under `Application.persistentDataPath`) |                           |

---

## 🔊 Sound Hooks

* Attach `SoundHook` to UI Buttons or triggers.
* Assign a `Sound` enum value (e.g., `Sound.Win`, `Sound.Arrow`).

import json
from pathlib import Path

CONFIG_FILE = Path("waves.json")
ENEMY_ASSETS = ["Enemy_01", "Enemy_02", "Enemy_03"]

def load_config():
    if CONFIG_FILE.exists():
        with open(CONFIG_FILE, "r") as f:
            return json.load(f)
    else:
        return {"waves": []}

def save_config(config):
    with open(CONFIG_FILE, "w") as f:
        json.dump(config, f, indent=4)

def list_waves(config):
    print("\n--- Current Waves ---")
    for i, wave in enumerate(config["waves"]):
        print(f"Wave {i}: prepareTime={wave['prepareTime']}, groups={len(wave['groups'])}, nextWave={wave['nextWave']}")

def add_wave(config):
    prepare = float(input("Prepare time (seconds): "))
    wave = {
        "prepareTime": prepare,
        "groups": [],
        "nextWave": -1
    }
    config["waves"].append(wave)
    print(f"Added new wave {len(config['waves'])-1}.")

def add_group(config):
    list_waves(config)
    idx = int(input("Select wave index: "))
    path_index = int(input("Path index: "))
    group = {"pathIndex": path_index, "squads": []}
    config["waves"][idx]["groups"].append(group)
    print(f"Added new group to wave {idx}.")

def add_squad(config):
    list_waves(config)
    idx = int(input("Select wave index: "))
    gidx = int(input("Select group index: "))
    
    # Choose from predefined assets
    print("Available enemy assets:")
    for i, asset in enumerate(ENEMY_ASSETS):
        print(f"{i}: {asset}")
    asset_idx = int(input("Select asset index: "))
    asset = ENEMY_ASSETS[asset_idx]
    
    count = int(input("Enemy count: "))
    squad = {"asset": asset, "count": count}
    config["waves"][idx]["groups"][gidx]["squads"].append(squad)
    print(f"Added {count} x {asset} to wave {idx}, group {gidx}.")

def main():
    config = load_config()

    while True:
        print("\nOptions: [L]ist waves, [AW] add wave, [AG] add group, [AS] add squad, [Q]uit")
        cmd = input("> ").strip().upper()

        if cmd == "L":
            list_waves(config)
        elif cmd == "AW":
            add_wave(config)
        elif cmd == "AG":
            add_group(config)
        elif cmd == "AS":
            add_squad(config)
        elif cmd == "Q":
            save_config(config)
            print("Saved and quitting.")
            break
        else:
            print("Unknown command.")

if __name__ == "__main__":
    main()

# Supcom2Cards by Alphahex

Card art made by Krawl

If you have any suggestions or find bugs, feel free to contact me on discord: Alphahex#4470

I receive messages from people I share servers with so maybe instead of sending a friend request just temporarily join: https://discord.gg/3xf6cRn or https://discord.gg/zUtsjXWeWk

Check out SC2 on Steam if you like RTS: https://store.steampowered.com/app/40100/Supreme_Commander_2/ and be sure to buy the DLC too! :)

## Cards

* Afterburn: Blocking doubles your Movement Speed for a few seconds
* Buhbledow: Dealing damage to somebody halfs the current recharge of their block meter unless full
* Chrome Shield: Automatically blocks before taking damage if block cooldown allows it
* Cluster Bomb: Bullets create tiny explosions after impact
* Colossus: Ridiculous health boost at the cost of your movement speed, jump height and block cooldown
* Darkenoid: Insane DMG and ATKSPD boosts, but you can only shoot down
* Disruptor: Bullets stun players near anything they hit
* Dynamic Power Shunt: Faster block recharge when standing still
* Harden: Blocking boosts your ATKSPD and Bullet speed for a few seconds
* Hunker: Increase block duration but move slower while blocking
* Jackhammer: Projectiles create massive explosions at the cost of your movement speed and ATKSPD
* Loyalist: Massive AMMO and ATKSPD increase at the cost of your DMG and HP
* Magnetron: Blocking can push or pull (and damage on contact) enemies depending on where you aim before blocking (up/down)
* Megalith: Continuously burn visible enemies with 2 lasers
* Nuke: You know what it does
* Overcharge: Blocking boosts your DMG, ATKSPD and makes bullets explode for a few seconds but large block cooldown increase
* Poseidon: Buckshot but not useless
* Quantum Sponge: Taking damage will partially recharge your block cooldown
* Radar Jammer: Enemy guns are less accurate while you're alive
* Recycler: Continuously steal HP from every enemy on screen while you're alive
* Rogue Nanites: Blocking heals 25 HP
* Shotja: Bullets travel WAY faster and also ignore gravity at the cost of some HP
* Stacked Cannons: Fire your entire clip instantly
* Super Triton: Bullet hitbox increased dramatically, more DMG, less ATKSPD
* Titan: Movement speed and jump height increased at the cost of some HP
* TML: Shoot pairs of bullets that ignore gravity and travel in straight lines at the cost of your ATKSPD
* Training: +25% HP and +25% DMG when you don't know what else to pick
* Veterancy: Every enemy kill permanently boosts your DMG/HP by 15% (+100% max per card)

## Changelog

#### v1.2.0
* SimpleAmount stats added
* Rebalanced: Buhbledow (halfs block meter instead of resetting to zero)
* Fixed block meter not being full on round start with Hunker

#### v1.1.9
* Megalith: Uncommon -> Rare
* Recycler: Uncommon -> Rare
* Recycler nerf
* Small optimizations

#### v1.1.8
* Fixed Darkenoid + Shields Up exploit
* Small optimizations on game load

#### v1.1.7
* Fixed effects persisting on Rematch

#### v1.1.5
* Cards added: Dynamic Power Shunt, Hunker
* Small optimizations and fixes

#### v1.1.4
* Fixed effects like Veterancy only lasting a single life (v1.1.3 bug)
* Updated Recycler with visuals

#### v1.1.3
* Card art updated
* Small optimizations and fixes

#### v1.1.2

* Fixed Veterancy breaking DealtDamage actions on other cards (example: Scavenger stops working)
* Fixed ridiculous Jackhammer explosion size
* Rebalanced: Veterancy (20% -> 15%)

#### v1.1.1

* Card art updated
* Cards added: Megalith, Veterancy
* Cards removed: Field Engineer (it kept causing issues and nobody used it anyway)
* Fixed Titan giving you 25% HP instead of -25% (oops xd)
* Fixed Radar Jammer effect not going away after death
* Fixed random number generators potentionally causing desync (example: every person having different cluster bomb explosions)
* Rebalanced: Cluster Bomb, Darkenoid, Jackhammer, Magnetron, Radar Jammer, Stacked Cannons
* ATKSPD stats changed from vanilla style that make no sense (example: -300% ATKSPD-> 25% ATKSPD)

#### v1.1.0

* Fixed none of the HP stats applying (v1.0.9 bug)

#### v1.0.9

* **DO NOT USE - MAJOR BUG!** None of the HP stats apply, fixed in v1.1.0
* Card art updated
* Fixed Harden applying extra bullet speed on every block
* ~~Fixed HP stats being applied incorrectly~~ nope, broke it instead
* Rebalanced: Colossus, Field Engineer

#### v1.0.8

* Cards added: Aterburn
* Fixed Overcharge using Harden stats for its duration
* Rebalanced: Super Triton, Titan

#### v1.0.7

* Rebalanced: Colossus, Magnetron

#### v1.0.6

* Card art updated
* Cards added: Darkenoid
* Rebalanced: Cluster Bomb, Magnetron (push/pull force decreased)

#### v1.0.5

* Card art updated
* Fixed issues with cards you can't take multiple times
* Fixed Quantum Sponge activating after Chrome Shield without taking damage
* Fixed GunProjectileSizePatch

#### v1.0.4

* Cards added: Buhbledow, Quantum Sponge, Stacked Cannons
* Rebalanced: Shotja, Titan

#### v1.0.3

* Fixed Magnetron DPS and force dependency on FPS and player count
* Optimized Cluster Bomb
* Fixed wrong block cooldowns on all cards that increased block cooldowns
* Rebalanced: Cluster Bomb, Magnetron

#### v1.0.2

* Cards added: Cluster Bomb
* Fixed explosion sounds on: Disruptor, Jackhammer, Nuke

#### v1.0.1

* Cards added: Chrome Shield, Colossus, Disruptor, Field Engineer, Harden, Jackhammer, Loyalist, Magnetron, Nuke, Overcharge, Poseidon, Radar Jammer, Recycler, Rogue Nanites, Shotja, Super Triton, Titan, TML, Training

#### v1.0.0

* **DO NOT USE - DOESN'T WORK!** I forgot to include a BepIn dependency, fixed in v1.0.1
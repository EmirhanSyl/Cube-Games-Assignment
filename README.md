# Cube-Games-Assignment
 An assignment for Cube Games

## Includes:

- Level system(3 level)
- 4 type of collectable objects(Coin, Diamond, TimeSlower, Shield)
- ObjectPooling System
- VFX effects on collect and hit events
- Upgrade system
- Save and continue system
- UI animations
- sound effects

### Interfaces:

#### Menu UserInterface(UI):
![image](https://user-images.githubusercontent.com/61618968/172655486-20d64d59-ba20-4121-801e-115ddb05f33e.png)

* The starting screen includes how much total game money players have and which level is active.
* PLUS: Swipe To Start panel and animation + Delete saves button

#### In-Game UserInterface(UI):
![image](https://user-images.githubusercontent.com/61618968/172656828-18efbf6e-ed52-4141-a144-cefa50c36715.png)

* The starting screen includes how much game money players collected, player health count and which level is active.
* PLUS: Level Progress bar + Active booster icon

#### Finish UserInterface(UI):

##### Fail UI:
![image](https://user-images.githubusercontent.com/61618968/172659356-8f2e48ce-ad03-4aff-b910-04c6ae224906.png)

* The fail screen includes how much game money players collected and will lose.
* PLUS: Lost money number animation + Retry button + Watch an add and continue to play button

##### Passed UI:
![image](https://user-images.githubusercontent.com/61618968/172661365-1434543d-b996-4026-939d-b2c3d8d3f1ce.png)

* The fail screen includes how much game money players collected and will earn and continue option.
* PLUS: Earned money number animation

## Optional Features:

### Pooling System:
![image](https://user-images.githubusercontent.com/61618968/172663190-cdf9f84a-0a1b-46d2-a0be-74455588fd94.png)

* Adding some gameobjects to object pools and reuse it. It's very important for optimization.
* **PLUS**: All collectables and obstacle objects are added pool

### VFX Effects:
![image](https://user-images.githubusercontent.com/61618968/172664076-5d3566e1-cd84-4830-b9a1-38a7e6f37f7d.png)![image](https://user-images.githubusercontent.com/61618968/172664180-a9e9af33-7b01-4a59-84e0-5c3f3dc8635e.png)

* Particle Effects used when collected a diamond or hitted an obstacle

### Additional Levels:
![image](https://user-images.githubusercontent.com/61618968/172664739-c620a150-802e-4f37-a531-4359af9e2bb6.png)

* Designing more then one level
* PLUS: Saving all level stats with PlayerPrefs

### Gameplay Improvements:

#### Shield:
![image](https://user-images.githubusercontent.com/61618968/172668253-bd53a8ab-997c-4c44-a8f5-a4b1448534ee.png)![image](https://user-images.githubusercontent.com/61618968/172668046-e8d1d6e4-b2c3-41ce-a482-239e57c91581.png)

* When shild collectable is taken, it provides an ability to doesn't effect from obstacles for player. For pretty short time, of course!

#### Time Slower:

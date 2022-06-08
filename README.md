# Cube-Games-Assignment
 An assignment for Cube Games

## About game:
* It's a basic 3D runner game. Player run forward and try to passing levels with collecting the collectables and escape from obstacles. 

## Includes:

- Level system(3 level)
- 4 type of collectable objects(Coin, Diamond, TimeSlower, Shield)
- ObjectPooling System
- VFX effects on collect and hit events
- Upgrade system
- Save and continue system
- UI animations
- sound effects
- AND MORE... (In the following lines)

### Swerve System
![image](https://user-images.githubusercontent.com/61618968/172684492-6cb6c115-4558-4e83-a9df-64991041e7c8.png)

* Useful and well performed swerve system. It changes player's x position depends on swipe moves on screen

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
![image](https://user-images.githubusercontent.com/61618968/172669621-5767c1eb-a3c3-4566-affc-a9396092813e.png)

* When this booster is collected, it slow down the time for 2 seconds. (2 secons for slow game and 4 seconds for real time)

### UI Animations:

* There are 3 animation in UI. 
1- When a diamond collect, diamond icon plays a little animation.

![image](https://user-images.githubusercontent.com/61618968/172670642-2c9bd41d-da48-42a3-877e-47bd2b52b3e5.png)

2- On failed canvas, lost diamonds and coins panels play an animation.

![image](https://user-images.githubusercontent.com/61618968/172670967-a0a64aa1-f869-436f-b15d-08d3a14b1e54.png)

3- On passed canvas, earned diamonds and coins panels and their total count icons play an animation.

![image](https://user-images.githubusercontent.com/61618968/172671415-c2ba341f-d8d2-4cac-8b22-57652c05abce.png)

### End Scene Cinematic Camera:
![image](https://user-images.githubusercontent.com/61618968/172677630-1b5d4961-8312-4423-b651-6843caa2fa42.png)

* When the level ends(player fail or pass) camera perspective changes smoothly.

### Upgrade Options:
![image](https://user-images.githubusercontent.com/61618968/172678166-5632d945-0698-4985-8b7c-710711498bc0.png)

* On the Menu canvas, there are 2 upgrade option for player. Basically Heart upgrade button increase player health count and players can have more chance to complate the level. And the second upgrade button increases collected coin amount multiplier. So, when player collect a coin it will multiply by the multiplier count.

## Bonus:

### Hitted and Failed Animations:
![image](https://user-images.githubusercontent.com/61618968/172679344-fc2de6b3-6877-4a97-bdb2-5e26cc416eca.png) ![image](https://user-images.githubusercontent.com/61618968/172679994-31347579-9484-4c63-8b4a-0cb4dc4d7574.png)

* Damage taken animation plays when player hits the obstacle and fail animation plays when player has no more heart at the end.

### Soundtrack And SFX

* A soundtrack that plays in entire game and a sound effect that plays when coin collects

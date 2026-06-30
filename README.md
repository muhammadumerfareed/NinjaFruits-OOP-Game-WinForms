
# 🍉⚔️ Ninja Fruits - OOP Arcade Game

Ninja Fruits is a 2D arcade game built in C# using the .NET framework and Windows Forms. Inspired by the classic mobile game, players control a ninja to slice falling fruits while avoiding deadly bombs. 

This project was built in a single-night sprint to meet an academic deadline, with a primary focus on practically implementing core Object-Oriented Programming (OOP) principles.

**Note:** This is Version 1.0. While fully playable, it contains some known bugs. Learning is a continuous journey, and future updates are planned to refactor the code and improve rendering efficiency!

## 🎮 Gameplay Mechanics
*   **Move:** Use `Left/Right Arrow Keys` (or A/D) to move the ninja across the bottom of the screen.
*   **Attack:** Press `Spacebar` to throw a sword vertically.
*   **Scoring:** Successfully slicing a fruit splits it in two and increases your score by 1.
*   **Health:** Letting a fruit drop past the screen without slicing it costs 1 health point.
*   **Game Over:** Hitting a bomb or dropping to 0 health ends the game immediately.

## 🏗️ Architecture & OOP Principles
The game avoids "spaghetti code" by utilizing a clean, highly structured class hierarchy:

*   **Abstraction:** An abstract `GameObject` base class defines the structure for all entities, forcing subclasses to implement their own `Update()` movement logic. The `Fruit` class is also abstract, branching into `LargeFruit` and `SmallFruit`.
*   **Encapsulation:** All attributes (position, speed, sprites) are private/protected and accessed via properties.
*   **Inheritance:** `Player`, `Sword`, `Bomb`, `SplashEffect`, and `Fruit` all inherit from `GameObject` to share common rendering and coordinate logic.
*   **Polymorphism:** The central `GameManager` handles the 25ms game loop by iterating through a generic `List<GameObject>`, dynamically invoking `Update()` and `Draw()` on each frame.
*   **Interfaces:** The `ICollidable` interface enforces a strict `OnCollision()` contract for intersection handling. 

## 🛠️ Tech Stack & Implementation Details
*   **Language/Framework:** C#, .NET 10.0, Windows Forms (WinForms).
*   **Rendering:** GDI+ with Double Buffering enabled on the main form to prevent screen flickering at ~40 FPS.
*   **Audio Management:** Custom `SoundManager` utilizing the Windows MCI API (`winmm.dll`) to allow concurrent, non-blocking sound effect playback during rapid slicing.

## 🚀 Installation & Setup
1. Clone this repository to your local machine.
2. Open the solution file in **Visual Studio 2022**.
3. Ensure you have the .NET 10.0 SDK installed.
4. Build and run the project (F5).

## 🗺️ Roadmap / Future Improvements
- [ ] Refactor codebase for cleaner separation of concerns.
- [ ] Fix known v1.0 physics and rendering bugs.
- [ ] Implement a difficulty scaling system (faster spawn rates as score increases).
- [ ] Add a persistent High Score leaderboard.
- [ ] Integrate mouse aiming for a more authentic swiping experience.

## 👤 Author
**Muhammad Umer Fareed**
*Department of Computer Science, University of Engineering and Technology (UET), Lahore*

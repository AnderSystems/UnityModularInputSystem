# Custom Input System

A custom input system that uses both the new and old Unity input systems.

## Table of Contents

- Features
- Installation
- Usage

## Features
This new input system is easy to use and configure, similar to old input system functionality, this is very adaptable and you can customize it

## Installation

To install this package, follow these steps:

1. Open Unity and go to **Window > Package Manager**.
2. Click the **+** button and select **Add package from git url** and paste ```https://github.com/AnderSystems/UnityModularInputSystem.git```.
3. Configure the inputs on **Project Settings > Input System**

## Usage

After installing the package, you can use the custom input system in your project. Here are some examples:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    void Update()
    {
        if (InputSystem.GetButtonDown("Jump"))
        {
            Debug.Log("Jump button pressed");
        }

        Vector2 move = InputSystem.GetAxis("Move");
        Debug.Log("Move axis: " + move);
    }
}

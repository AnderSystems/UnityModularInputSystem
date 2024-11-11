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

1. Donload the latest version
2. In Project Window, `Right Click > Import Package > Custom Package` and Select the Input Package
3. Configure your inputs in ``ProjectSettings > Input System``

## Usage

After installing the package, you can use the custom input system in your project. Here are some examples:

```csharp
using UnityEngine;

public class Example : MonoBehaviour
{
    void Update()
    {
        if (GameInput.GetButtonDown("Jump"))
        {
            Debug.Log("Jump button pressed");
        }

        Vector2 move = GameInput.GetAxis("Move");
        Debug.Log("Move axis: " + move);
    }
}

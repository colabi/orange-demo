# orange-demo

TIC TAC TOE in an air projected screen.  Supports 3 modes of play.


## Instructions

Find a nice floor in the camera view and tap to instantiate the air projection on the ground.  *Air Mouse* is a center view selector.  To choose gameplay between human vs. human, human vs. machine, and machine vs. machine modes, center your choice in the camera and tap on the air projected screen.  To select a square in the game, center the empy box of your choice and tap.

UI is limited, so to reset to home screen (and choose a different mode), center on the box projector root on the ground and tap on it.

Can be played in editor.  Mouse simulates touch but is not center constrained.

## Binary build
An Android P build is available in  /builds directory

## Bugs
Currently, there is a bug I'm tracking down that regards detecting winning states.

## General architecture highlights
Actor entities (human, computer) acquire Skill objects from a blindly configured factory to interact with an Interaction Context (Turn based game in this instance).  Lots of this is indicated but not fully implemented.  

The context (ideally) interacts with objects in the environment through a not at all completely fleshed out IO system (touch input/monitor output).  Even the computer must simulate a touch on the virtual input device.  As such, it is loosely coupled and intentionally open to cheating. 

The AI uses a simple minimax algorithm for the game.  To be better implemented (CPU/RAM) on things like initial move, the decision tree could be baked into a blob that is bit packed.  Wasn't time for this.  But on a device like a mobile you can see a stall on the first move (as it computes the full 500K+ moves).


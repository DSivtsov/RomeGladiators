Every character have own StateMachine (SM) called from SingletonGladiatorsManager()
The all state, transation with conditions initialized for every character to reduce overhead at calling with parameters and stored in evere character (Gladiator())
All States realized IState
All functionallity deviede between StateMachine() and Gladiator()
SM cycle begin from method StateMachineTick()
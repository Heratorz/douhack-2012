import QtQuick 1.1

Sprite {
    id: megaCat
    width: 512
    height: 512

    animationName: "sit"
    frames: 1
    repeat: true

    states: [
        State {
            name: "EnteringState"
            PropertyChanges {
                target: megaCat
                animationName: "vxod"
                frames: 24
                repeat: false
                interval: 150
            }
        },
        State {
            name: "FacepalmState"
            PropertyChanges {
                target: megaCat
                animationName: "fp"
                frames: 12
                repeat: false
                interval: 150
                onAnimationEnded: megaCat.state = ""
            }
        },
        State {
            name: "MimimiState"
            PropertyChanges {
                target: megaCat
                animationName: "mimimi"
                frames: 3
                repeat: false
                interval: 2000
                onAnimationEnded: megaCat.state = ""
            }
        },
        State {
            name: "WinkState"
            PropertyChanges {
                target: megaCat
                animationName: "lyp"
                frames: 4
                repeat: false
                interval: 150
                onAnimationEnded: megaCat.state = ""
            }
        },
        State {
            name: "StandState"
            PropertyChanges {
                target: megaCat
                animationName: "to_stand"
                frames: 1
                repeat: false
                interval: 50
                onAnimationEnded: megaCat.state = "ScaryState"
            }
        },
        State {
            name: "ScaryState"
            PropertyChanges {
                target: megaCat
                animationName: "ispyg"
                frames: 4
                repeat: true
                repeatCount: 5
                interval: 50
                onAnimationEnded: megaCat.state = "SitState"
            }
        },
        State {
            name: "SitState"
            PropertyChanges {
                target: megaCat
                animationName: "to_sit"
                frames: 2
                repeat: false
                running: true
                interval: 100
                onAnimationEnded: megaCat.state = ""
            }
        }
    ]
    state: ""

    MouseArea {
        id: mouseArea
        anchors.fill: parent
        onClicked: {
            megaCat.state = megaCat.state == ""? "StandState" : ""
        }
    }
}

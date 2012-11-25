import QtQuick 1.1
import "loader.js" as L

Sprite {
    id: megaCat

    property bool isAbsent: false

    function feed() {
        loyaltyText.text -= 10;
        L.decreaseLoyalty();
    }

    width: 512
    height: 512

    animationName: "sit"
    frames: 1
    repeat: true
    imageOpacity: 1.0
    overlayOpacity: 0.0

    transitions: [
        Transition {
            from: "*"; to: "ExitState"
            ParallelAnimation{
                NumberAnimation { target: megaCat; property: "imageOpacity"; duration: 2000; from: 1.0; to: 0.0 }
                SequentialAnimation {
                    NumberAnimation { target: megaCat; property: "overlayOpacity"; duration: 2000; from: 1.0; to: 0.5 }
                    ScriptAction { script: megaCat.overlayOpacity = 0.0 }
                }
            }
        }
    ]

    states: [
        State {
            name: "ExitState"; when: megaCat.isAbsent
            PropertyChanges {
                target: megaCat
                imageOpacity: 0.0
                overlayOpacity: 0.0
            }
        },
        State {
            name: "EnteringState"
            PropertyChanges {
                target: megaCat
                animationName: "vxod"
                frames: 12
                repeat: false
                interval: 150
                onAnimationEnded: {
                    megaCat.state = (loyaltyText.text < 0)? "FacepalmState" : "ShiftState"
                    megaCat.isAbsent = false;
                }
            }
        },
        State {
            name: "ShiftState"
            PropertyChanges {
                target: megaCat
                animationName: "shift"
                frames: 2
                repeat: true
                repeatCount: 6
                interval: 150
                onAnimationEnded: megaCat.state = ""
            }
        },
        State {
            name: "RepeatState"
            PropertyChanges {
                target: megaCat
                animationName: "shift"
                frames: 2
                repeat: true
                repeatCount: 3
                interval: 150
                onAnimationEnded: megaCat.state = ""
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
            name: "ExitAfterFacepalmState"
            PropertyChanges {
                target: megaCat
                animationName: "fp"
                frames: 12
                repeat: false
                interval: 150
                onAnimationEnded: megaCat.isAbsent = true
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
        },
        State {
            name: "FeedState"
            PropertyChanges {
                target: megaCat
                animationName: "mimimi"
                frames: 1
                repeat: false
                running: true
                interval: 2000
                onAnimationEnded: megaCat.state = ""
            }
        }
    ]
    state: ""

    Timer {
        interval: 1000 * 15
        repeat: true
        running: true
        onTriggered: {
            if (megaCat.state == "")
                megaCat.state = "RepeatState";
        }
    }

//    MouseArea {
//        id: mouseArea
//        anchors.fill: parent
//        onClicked: {
//            megaCat.isAbsent = !megaCat.isAbsent;
//            if (!megaCat.isAbsent)
//                megaCat.state = "EnteringState";
//        }
//    }
}

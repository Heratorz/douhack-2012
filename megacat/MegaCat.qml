import QtQuick 1.1

Sprite {
    id: megaCat
    width: 512
    height: 512

    animationName: "proba"
    frames: 13
    repeat: true

    states: [
//        State {
//            name: "SittingState"
//            PropertyChanges {
//                target: megaCat
//                animationName: "proba"
//                frames: 13
//            }
//        },
        State {
            name: "EnteringState"
            PropertyChanges {
                target: megaCat
                animationName: "vxod"
                frames: 6
                repeat: false
            }
        }
    ]
    state: ""

    MouseArea {
        id: mouseArea
        anchors.fill: parent
        onClicked: {
            megaCat.state = megaCat.state == ""? "EnteringState" : ""
        }
    }
}

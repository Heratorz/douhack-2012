import QtQuick 1.1
import "loader.js" as L

Rectangle {
    width: 512
    height: 512

    color: "#400040"

    Rectangle {
        id: counterRect
        width: parent.width * 0.125
        height: parent.height * 0.125
        color: "orange"
        x: width / 2
        y: height / 2

        Text {
            id: loyaltyText

            text: "100"
            color: "#400040"
            smooth: true

            anchors {
                fill: parent
                rightMargin: 5
            }

            font {
                family: "Arial Black"
                pointSize: 16
                bold: true
            }

            horizontalAlignment: Text.AlignRight
            verticalAlignment: Text.AlignVCenter

            Timer {
                interval: 3000
                repeat: true
                running: true
                triggeredOnStart: true
                onTriggered: L.getLoyalty();
            }
        }

        states: [
            State {
                name: "PositiveState"
                PropertyChanges {
                    target: counterRect
                    color: Qt.lighter("green")
                }
            },
            State {
                name: "NegativeState"
                PropertyChanges {
                    target: counterRect
                    color: Qt.lighter("red")
                }
            }
        ]

        transitions: [
            Transition {
                from: "*"; to: ""
                SequentialAnimation {
                    ColorAnimation { easing.type: Easing.InQuad; from: counterRect.color; to: "orange"; duration: 400 }
                }
            }
        ]
    }

    MegaCat {
        id: megaCat
        width: 512
        height: 512
        anchors.centerIn: parent
    }

    Text {
        text: "Give fish"
        color: Qt.lighter("#400040")

        width: 70
        height: 20

        font.pointSize: 16

        anchors {
            left: parent.left
            bottom: parent.bottom
            leftMargin: 10
            bottomMargin: 10
        }

        MouseArea {
            id: mouseArea
            anchors.fill: parent
            onClicked: { megaCat.feed(); }
        }
    }
}

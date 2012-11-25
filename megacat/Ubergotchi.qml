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

            text: "0"
            color: "#400040"

            anchors {
                fill: parent
                rightMargin: 5
            }

            font {
                family: "Arial Black"
                pixelSize: 30
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
        width: 512
        height: 512
        anchors.centerIn: parent
    }
}
